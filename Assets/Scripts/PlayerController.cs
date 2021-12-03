using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	enum JumpDir
	{
		Left,
		Right,
		Up,
		Down,
	};

	[Header("Movement")]
	[SerializeField] float movementSpeed = 0.6f;
	[SerializeField] float jumpForce = 2.3f;
	[SerializeField] float reverseJumpForce = 0.05f;
	[SerializeField] float maxJumpReverseVelocity = 0.1f;
	[SerializeField] float jumpCoolDown = 0.15f;

	[Header("Ground Check (for jumping)")]
	[SerializeField] List<Transform> groundChecks;
	[SerializeField] float groundChecksRadius = 0.05f;
	[SerializeField] LayerMask groundCheckLayerMask;

	float lastJumpTime = 0;
	bool didJump = true;
	JumpDir jumpDir = JumpDir.Up;
	bool didReverseJump = false;

	Rigidbody2D rb;
	List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

	Light2D playerLight;
	[Header("Lightning")]
	[SerializeField] float intensityAddition = 0.1f;
	[SerializeField] Vector2 intensityMinMax = new Vector2(0.9f, 1.1f);
	[SerializeField] float lightChangeChance = 5;
	[SerializeField] Vector2 minMaxR = new Vector2(200f, 255f);
	[SerializeField] Vector2 minMaxG = new Vector2(134f, 172f);

	[Header("Audio")]
	[SerializeField] List<AudioClip> ambientAudio;
	[SerializeField] float ambientVolume;
	[SerializeField] List<AudioClip> jumpAudio;
	[SerializeField] float jumpAudioVolume;

	AudioSource ambientAudioSource;
	AudioSource jumpAudioSource;

	void setFlipped(bool isFlipped)
	{
		foreach (var r in spriteRenderers)
			r.flipX = isFlipped;
	}

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		GetComponentsInChildren<SpriteRenderer>(spriteRenderers);
		playerLight = GetComponent<Light2D>();

		ambientAudioSource = gameObject.AddComponent<AudioSource>();
		ambientAudioSource.volume = ambientVolume;
		jumpAudioSource = gameObject.AddComponent<AudioSource>();
		jumpAudioSource.volume = jumpAudioVolume;

		ambientAudioSource.volume = 0.558f;
		ambientAudioSource.clip = ambientAudio[0];
		ambientAudioSource.loop = true;
		ambientAudioSource.Play();

		jumpAudioSource.volume = 0.87f;
	}

	void Update()
	{
		if (Mathf.Round(Random.Range(0f, lightChangeChance)) == 0)
		{
			Color c = playerLight.color;
			c.r = Random.Range(minMaxR.x / 255f, minMaxR.y / 255f);
			c.g = Random.Range(minMaxG.x / 255f, minMaxG.y / 255f);
			playerLight.color = c;

			float f = Random.Range(intensityMinMax.x, intensityMinMax.y);

			playerLight.pointLightOuterRadius = f;
			playerLight.intensity = f + intensityAddition;
		}

		if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
			setFlipped(true);
		else if (!Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
			setFlipped(false);
	}

	void FixedUpdate()
	{
		Vector2 vel = rb.velocity;

		bool canJump = CanJump();

		if (didJump && canJump)
		{
			didJump = false;
			didReverseJump = false;
		}

		if (didJump)
		{
			if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
			{
				if (jumpDir != JumpDir.Left)
					didReverseJump = true;

				if (didReverseJump || jumpDir == JumpDir.Up || jumpDir == JumpDir.Down)
				{
					if (vel.x > -maxJumpReverseVelocity)
						vel.x -= reverseJumpForce;
				}
			}
			else if (!Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
			{
				if (jumpDir != JumpDir.Right)
				{
					didReverseJump = true;
				}

				if (didReverseJump || jumpDir == JumpDir.Up || jumpDir == JumpDir.Down)
				{
					if (vel.x < maxJumpReverseVelocity)
						vel.x += reverseJumpForce;
				}
			}
		}
		else
		{
			if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
			{
				vel.x = -movementSpeed;
			}
			else if (!Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
			{
				vel.x = movementSpeed;
			}
			else
			{
				vel.x = 0f;
			}
		}

		if (Input.GetKey(KeyCode.Space) && canJump)
		{
			lastJumpTime = Time.time + jumpCoolDown;
			vel.y = jumpForce;
			jumpAudioSource.PlayOneShot(jumpAudio[Random.Range(0, jumpAudio.Count)], jumpAudioVolume);
			didJump = true;

			if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
				jumpDir = JumpDir.Left;
			else if (!Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
				jumpDir = JumpDir.Right;
			else if (Input.GetKey(KeyCode.DownArrow))
				jumpDir = JumpDir.Down;
			else
				jumpDir = JumpDir.Up;
		}

		rb.velocity = vel;
	}

	bool CanJump()
	{
		if ((lastJumpTime < Time.time) && (rb.velocity.y <= 0.05f))
		{
			foreach (var gc in groundChecks)
				if (Physics2D.OverlapCircle(gc.position, groundChecksRadius, groundCheckLayerMask) != null)
					return true;
		}
		return false;
	}
}
