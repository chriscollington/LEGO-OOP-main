using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.LEGO.UI
{
    public class CollectFollow : MonoBehaviour
    {
        public GameObject player; // Reference to the player GameObject
        public GameObject uiTarget; // Reference to the UI GameObject (target position)
        /*
        Make sure the uiTarget canvas has a negative sorting layer so it sits behind the default UI
        */
        public GameObject coin2DPrefab; // Reference to the 2D coin prefab
        public float StartSpeed = 45; // Initial speed for SmoothDamp
        public float EndSpeed = 35; // Final speed for SmoothDamp
        public float followDuration = 10f; // Time to follow the player
        public float movementSpeed = 1000f; // Speed for the 2D coin's movement (pixels/second)

        private Vector3 _velocity = Vector3.zero;
        private bool _isFollowing = false;


        // Speed of movement (duration for the move)
        public float moveDuration = 1f;

     
        void Start()
        {
            // Ensure references are assigned
            if (player == null)
                Debug.LogError("Player reference is not assigned!");

            if (uiTarget == null)
                Debug.LogError("UI Target reference is not assigned!");

            if (coin2DPrefab == null)
                Debug.LogError("2D Coin Prefab reference is not assigned!");

            

  
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject == player)
            {
                StartCoroutine(FollowPlayerThenCreate2DCoin());
                Debug.Log("Triggered by player");
            }
        }

        private IEnumerator FollowPlayerThenCreate2DCoin()
        {
            _isFollowing = true;

            // Follow the player for the specified duration
            yield return new WaitForSeconds(followDuration);

            _isFollowing = false;

            // Create a 2D coin copy and animate it to the UI target
            CreateAndMove2DCoin();

            // Disable the 3D coin
            gameObject.SetActive(false);

            
        }


         // Coroutine that moves the sprite to the target position over time
        void Update()
        {
            if (_isFollowing)
            {
                // Follow the player smoothly
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    player.transform.position,
                    ref _velocity,
                    Time.deltaTime * Random.Range(EndSpeed, StartSpeed)
                );
            }
        }



        private void CreateAndMove2DCoin()
        {
            // Convert the 3D coin's position to screen space
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

            // Instantiate the 2D coin prefab as a child of the Canvas
            GameObject new2DCoin = Instantiate(coin2DPrefab, uiTarget.transform.parent);

            // Set its initial position and scale
            RectTransform rectTransform = new2DCoin.GetComponent<RectTransform>();
            rectTransform.position = screenPosition;
            rectTransform.localScale = Vector3.one;

            new2DCoin.SetActive(true);
            
            DeleteCoin(new2DCoin);

        }

        private void DeleteCoin(GameObject coin)
        {
            //Debug.Log("Deleting " + coin.name + " after 2 seconds.");
            Destroy(coin, 2f);
        }
       
    }
}
