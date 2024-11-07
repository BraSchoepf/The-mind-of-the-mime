using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformSystem
{

    public class Platform : MonoBehaviour
    {
        protected Rigidbody2D rb;
        protected SpriteRenderer spriteRenderer;


        [SerializeField] protected bool isVisible = true;
        [SerializeField] protected bool isMoving = false;
        [SerializeField] protected bool fallsOnContact = false;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            if (!isVisible)
            {
                spriteRenderer.enabled = false;
                gameObject.SetActive(false);

            }
            else
            {
                spriteRenderer.enabled = true;
            }

            if (rb != null)
            {
                rb.bodyType = !isMoving ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
            }
            static void ActivatePlatform(Platform @this)
            {
                @this.
                            gameObject.SetActive(true);
                @this.spriteRenderer.enabled = true;

                if (@this.isMoving && @this.rb != null)
                {
                    @this.EnableMovement();
                }
            }
        }




        public virtual void EnableMovement()
        {
            if (rb != null && isMoving)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }

        public void SetStaticCollision()
        {
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Static;
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (fallsOnContact && collision.gameObject.CompareTag("Player"))
            {
                Fall();
            }
        }

        protected virtual void Fall()
        {
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 1.5f;
            }
        }
    }
}

