using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using KartGame.KartSystems;
using NaughtyAttributes;

public class CocheBazinga : Agent, IInput
{
	public enum AgentMode
	{
		Training,
		Inferencing
	}

	#region Training Modes
	[Header("Agent Mode")]
	public AgentMode Mode = AgentMode.Training;
	#endregion

	#region Rewards
	[Header("Rewards")]
	public float HitPenalty = -1f;
	public float PassCheckpointReward;
	public float SpeedReward;
	public float AccelerationReward;
    public float WrongCheckpointPenalty;
	#endregion

	#region ResetParams
	[Header("Reset Params")]
	public LayerMask OutOfBoundsMask;
	public LayerMask TrackMask;
	public float GroundCastDistance;
	public float offsetSpawn;
	#endregion

	#region Senses
	[Header("Observation Params")]
	public Transform AgentSensorTransform;
	[Range(1, 1000)] public int rayLength = 10;
	#endregion

	#region Checkpoints
	[Header("Training Params")]
	public Transform[] CheckpointsParents;
	public bool manualIndexTrack;
	[ShowIf("manualIndexTrack")] public int indexTrack;
	public bool manualIndexCheckpoint;
	[ShowIf("manualIndexCheckpoint")] public int indexCheckpoint;
	public LayerMask CheckpointMask;
	public bool randomDirection;
	int nCheckpoints;
	int CheckpointsLap;
	[MinValue(1)] public int Laps;
	Transform m_LastCheckpoint;
	#endregion

	#region Debugging
	[Header("Debug Option")]
	public bool ShowRaycasts;
	#endregion

	#region Visual
	[Header("Visual Attributes")]
	public SkinnedMeshRenderer kart;
	public SkinnedMeshRenderer character;
	#endregion

	ArcadeKart m_Kart;
	KeyboardInput manualInput;
	bool m_Acceleration;
	bool m_Brake;
	float m_Steering;

	BehaviorParameters parameters;

	private void Awake()
	{
		m_Kart = GetComponent<ArcadeKart>();
		if (AgentSensorTransform == null) AgentSensorTransform = transform;
		parameters = GetComponent<BehaviorParameters>();

		manualInput = GetComponent<KeyboardInput>();

		kart.material.color = Random.ColorHSV(0f,1f, 1f,1f ,1f,1f);
		character.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
	}

	public override void OnEpisodeBegin()
	{
		var parent = manualIndexTrack ? CheckpointsParents[indexTrack] : CheckpointsParents[Random.Range(0, CheckpointsParents.Length)];
		var checkpoint = manualIndexCheckpoint ? parent.GetChild(indexCheckpoint) : parent.GetChild(Random.Range(1, parent.childCount - 1));

		transform.localRotation = checkpoint.transform.rotation;
		transform.position = checkpoint.transform.position + checkpoint.transform.forward * offsetSpawn;

		if (randomDirection && Random.Range(0f, 1f) < .5f)
		{
			transform.Rotate(new Vector3(0f, 180f), Space.Self);
			transform.position = checkpoint.transform.position - checkpoint.transform.forward * offsetSpawn;
		}

		m_Kart.Rigidbody.velocity = default;
		m_Acceleration = false;
		m_Brake = false;
		m_Steering = 0f;
		m_LastCheckpoint = checkpoint;
		nCheckpoints = 0;
		CheckpointsLap = parent.childCount * Laps;
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation(m_Kart.LocalSpeed());
		sensor.AddObservation(m_Acceleration);

		for (var i = 0; i < AgentSensorTransform.childCount; i++)
		{
			var pos = AgentSensorTransform.position;
			var angle = AgentSensorTransform.GetChild(i).forward;

			var hit = Physics.Raycast(pos, angle, out var hitInfo, rayLength, TrackMask);
			sensor.AddObservation(hit ? hitInfo.distance : rayLength);

			if (ShowRaycasts)
				if (hit)
					Debug.DrawRay(pos, angle * hitInfo.distance, Color.red);
				else
					Debug.DrawRay(pos, angle * hitInfo.distance, Color.green);

			var hit2 = Physics.Raycast(pos, angle, out var hitInfo2, rayLength, CheckpointMask);
			sensor.AddObservation(hit2 ? hitInfo2.distance : -1f);

			if (ShowRaycasts)
				if (hit2 && hitInfo2.distance < hitInfo.distance)
					Debug.DrawRay(pos, angle * hitInfo2.distance, Color.magenta);
		}

		sensor.AddObservation(m_LastCheckpoint == null ? 100f : Vector3.Distance(transform.position, m_LastCheckpoint.position));
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		ActionSegment<int> discreteSegment = actionsOut.DiscreteActions;

		var input = manualInput.GenerateInput();

		m_Steering = input.TurnInput;
		m_Acceleration = input.Accelerate;
		m_Brake = input.Brake;
	}

	public override void OnActionReceived(ActionBuffers actions)
	{
		if (parameters.BehaviorType != BehaviorType.HeuristicOnly)
			InterpretDiscreteActions(actions.DiscreteActions);

		AddReward((m_Acceleration && !m_Brake ? 1.0f : 0.0f) * AccelerationReward); //reward por aceleracion
        AddReward(m_Kart.LocalSpeed() * SpeedReward); //reward por velocidad
    }

	private void LateUpdate()
	{
		
		if (ShowRaycasts)
			Debug.DrawRay(transform.position, Vector3.down * GroundCastDistance, Color.cyan);
        //si se sale del mapa acaba episodio
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out var hit, GroundCastDistance, OutOfBoundsMask))
			EndEpisode();
	}

	void InterpretDiscreteActions(ActionSegment<int> actions)
	{
		m_Steering = actions[0] - 1f;
		m_Acceleration = actions[1] >= 1.0f;
		m_Brake = actions[1] < 1.0f;
	}

	public InputData GenerateInput()
	{
		return new InputData
		{
			Accelerate = m_Acceleration,
			Brake = m_Brake,
			TurnInput = m_Steering
		};
	}

	void OnTriggerEnter(Collider other)
	{
		var maskedValue = 1 << other.gameObject.layer;
		var triggered = maskedValue & CheckpointMask;

		if (triggered > 0)
			if (other.name == m_LastCheckpoint.name)
			{
				AddReward(WrongCheckpointPenalty); //penaliza duramente y reinicia episodio
				if (Mode == AgentMode.Training)
					EndEpisode();
			}
			else
			{
				AddReward(PassCheckpointReward); //reward por camino correcto
				m_LastCheckpoint = other.transform;
                if (++nCheckpoints >= CheckpointsLap)
					EndEpisode();
			}

		var trackBorder = maskedValue & TrackMask;
		if (trackBorder > 0)
		{
			AddReward(HitPenalty);
			if (Mode == AgentMode.Training)
				EndEpisode();
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (!Application.isPlaying && ShowRaycasts)
			for (var i = 0; i < AgentSensorTransform.childCount; i++)
				Gizmos.DrawRay(AgentSensorTransform.position, AgentSensorTransform.GetChild(i).forward * rayLength);
	}
}
