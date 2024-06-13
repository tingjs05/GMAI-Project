/*
 * Copyright (c) 2019 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Linq;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class Character : MonoBehaviour, IDamagable
    {
        #region Variables

        #pragma warning disable 0649
        [SerializeField]
        private Transform handTransform;
        [SerializeField]
        private Transform sheathTransform;
        [SerializeField]
        private Transform shootTransform;
        [SerializeField]
        private CharacterData data;
        [SerializeField]
        private LayerMask whatIsGround;
        [SerializeField]
        private Collider hitBox;
        [SerializeField]
        private Animator anim;
        [SerializeField]
        private ParticleSystem shockWave;
        #pragma warning restore 0649
        [SerializeField]
        private float meleeRestThreshold = 10f;
        [SerializeField]
        private float diveThreshold = 1f;
        [SerializeField]
        private float collisionOverlapRadius = 0.1f;

        private GameObject currentWeapon;
        private Quaternion currentRotation;
        private int horizonalMoveParam = Animator.StringToHash("H_Speed");
        private int verticalMoveParam = Animator.StringToHash("V_Speed");
        private int shootParam = Animator.StringToHash("Shoot");
        private int hardLanding = Animator.StringToHash("HardLand");
        private int hitParam = Animator.StringToHash("Hit");
        #endregion

        #region Properties

        public float NormalColliderHeight => data.normalColliderHeight;
        public float CrouchColliderHeight => data.crouchColliderHeight;
        public float DiveForce => data.diveForce;
        public float JumpForce => data.jumpForce;
        public float MovementSpeed => data.movementSpeed;
        public float CrouchSpeed => data.crouchSpeed;
        public float RotationSpeed => data.rotationSpeed;
        public float CrouchRotationSpeed => data.crouchRotationSpeed;
        public GameObject MeleeWeapon => data.meleeWeapon;
        public GameObject ShootableWeapon => data.staticShootable;
        public float DiveCooldownTimer => data.diveCooldownTimer;
        public float RollSpeed => data.rollSpeed;
        public float CollisionOverlapRadius => collisionOverlapRadius;
        public float DiveThreshold => diveThreshold;
        public float MeleeRestThreshold => meleeRestThreshold;
        public int SwingMelee => Animator.StringToHash("SwingMelee");
        public int DrawMelee => Animator.StringToHash("DrawMelee");
        public int SheathMelee => Animator.StringToHash("SheathMelee");
        public int isMelee => Animator.StringToHash("IsMelee");
        public int crouchParam => Animator.StringToHash("Crouch");
        public int rollParam => Animator.StringToHash("Roll");

        public Rigidbody rb { get; private set; }
        public bool isSheathed { get; private set; } = true;
        public float Health { get; private set; }

        public float ColliderSize
        {
            get => GetComponent<CapsuleCollider>().height;

            set
            {
                GetComponent<CapsuleCollider>().height = value;
                Vector3 center = GetComponent<CapsuleCollider>().center;
                center.y = value / 2f;
                GetComponent<CapsuleCollider>().center = center;
            }
        }

        #endregion

        #region FSM

        // default states
        public StateMachine movementSM { get; private set; }
        public StandingState standing { get; private set; }
        public DuckingState ducking { get; private set; }
        public JumpingState jumping { get; private set; }
        public RollState roll { get; private set; }

        // attack states
        public StateMachine attackSM { get; private set; }
        public WeaponIdleState weaponIdle { get; private set; }
        public DrawState draw { get; private set; }
        public SheathState sheath { get; private set; }
        public AttackState attack { get; private set; }
        public AttackState1 attack1 { get; private set; }
        public AttackState2 attack2 { get; private set; }

        #endregion

        #region Methods
        public void Damage(float damage)
        {
            Health -= damage;
            TriggerAnimation(hitParam);
        }

        public void Move(float speed, float rotationSpeed, bool playAnim = true)
        {
            Vector3 targetVelocity = speed * transform.forward * Time.deltaTime;
            targetVelocity.y = rb.velocity.y;
            rb.velocity = targetVelocity;

            rb.angularVelocity = rotationSpeed * Vector3.up * Time.deltaTime;

            if (targetVelocity.magnitude > 0.01f || rb.angularVelocity.magnitude > 0.01f)
            {
                SoundManager.Instance.PlayFootSteps(Mathf.Abs(speed));
            }

            // check if need to play animation before playing walking/running animation
            if (!playAnim) return;
            anim.SetFloat(horizonalMoveParam, rb.angularVelocity.y);
            anim.SetFloat(verticalMoveParam, speed * Time.deltaTime);
        }

        public void ResetMoveParams()
        {
            rb.angularVelocity = Vector3.zero;
            anim.SetFloat(horizonalMoveParam, 0f);
            anim.SetFloat(verticalMoveParam, 0f);
        }

        public void ApplyImpulse(Vector3 force)
        {
            rb.AddForce(force, ForceMode.Impulse);
        }

        public void SetAnimationBool(int param, bool value)
        {
            anim.SetBool(param, value);
        }

        public void TriggerAnimation(int param)
        {
            anim.SetTrigger(param);
        }

        public AnimatorStateInfo GetAnimationState(int layer)
        {
            return anim.GetCurrentAnimatorStateInfo(layer);
        }

        public void Shoot()
        {
            TriggerAnimation(shootParam);
            GameObject shootable = Instantiate(data.shootableObject, shootTransform.position, shootTransform.rotation);
            shootable.GetComponent<Rigidbody>().velocity = shootable.transform.forward * data.bulletInitialSpeed;
            SoundManager.Instance.PlaySound(SoundManager.Instance.shoot, true);
        }

        public bool CheckCollisionOverlap(Vector3 point)
        {
            return Physics.OverlapSphere(point, CollisionOverlapRadius, whatIsGround).Length > 0;
        }

        public void Equip(GameObject weapon = null)
        {
            if (currentWeapon == weapon)
            {
                return;
            }
            else if (weapon != null)
            {
                if (currentWeapon != null) Unequip();
                currentWeapon = Instantiate(weapon, handTransform.position, handTransform.rotation, handTransform);
            }
            else
            {
                ParentCurrentWeapon(handTransform);
            }

            isSheathed = false;
        }

        public void DiveBomb()
        {
            TriggerAnimation(hardLanding);
            SoundManager.Instance.PlaySound(SoundManager.Instance.hardLanding);
            shockWave.Play();
        }

        public void SheathWeapon()
        {
            isSheathed = true;
            ParentCurrentWeapon(sheathTransform);
        }

        public void Unequip()
        {
            isSheathed = true;
            Destroy(currentWeapon);
        }

        public void ActivateHitBox()
        {
            hitBox.enabled = true;
        }

        public void DeactivateHitBox()
        {
            hitBox.enabled = false;
        }

        private void ParentCurrentWeapon(Transform parent)
        {
            if (currentWeapon.transform.parent == parent)
            {
                return;
            }

            currentWeapon.transform.SetParent(parent);
            currentWeapon.transform.localPosition = Vector3.zero;
            currentWeapon.transform.localRotation = Quaternion.identity;
        }
        #endregion

        #region MonoBehaviour Callbacks
        
        private void Start()
        {
            // get rigidbody component
            rb = GetComponent<Rigidbody>();
            // set health
            Health = data.maxHealth;

            // set up FSMs
            // default states
            movementSM = new StateMachine();
            standing = new StandingState(this, movementSM);
            ducking = new DuckingState(this, movementSM);
            jumping = new JumpingState(this, movementSM);
            roll = new RollState(this, movementSM);
            movementSM.Initialize(standing);

            // attack states
            attackSM = new StateMachine();
            weaponIdle = new WeaponIdleState(this, attackSM);
            draw = new DrawState(this, attackSM);
            sheath = new SheathState(this, attackSM);
            attack = new AttackState(this, attackSM);
            attack1 = new AttackState1(this, attackSM);
            attack2 = new AttackState2(this, attackSM);
            attackSM.Initialize(weaponIdle);
            // equip and sheath default melee weapon
            Equip(MeleeWeapon);
            SheathWeapon();
        }

        private void Update()
        {
            movementSM?.Update();
        }
        private void FixedUpdate()
        {
            movementSM?.FixedUpdate();
        }

        #endregion
    }
}
