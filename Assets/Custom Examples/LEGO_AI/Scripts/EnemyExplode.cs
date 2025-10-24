using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace Unity.LEGO.Minifig
{
    public class EnemyExplode : MonoBehaviour
    {
        public AudioClip explodeAudioClip;
        protected AudioSource audioSource;

        [SerializeField, HideInInspector]
        protected Transform leftArmTip = null;
        [SerializeField, HideInInspector]
        protected Transform rightArmTip = null;
        [SerializeField, HideInInspector]
        protected Transform leftLegTip = null;
        [SerializeField, HideInInspector]
        protected Transform rightLegTip = null;
        [SerializeField, HideInInspector]
        protected Transform head = null;

        protected EnemyMinifig minifig;

        protected Animator animator;
        Animator[] animators;

        Vector3 externalMotion;

        float externalRotation;
        protected float rotateSpeed;

        protected Vector3 moveDelta = Vector3.zero;

        protected bool exploded;


     [Header("Explosion Settings")]
    public float explosionForce = 0.1f;
    public float explosionRadius = 1f;
    public UnityEngine.Vector3 explosionOffset = UnityEngine.Vector3.zero; // Offset for the explosion center
    public float upwardModifier = 0.01f;
    public float destructionDelay = 5f; // Time before destroying the parent GameObject

    [Header("Target Group")]
    public string geoGroupName = "Geo_grp"; // Name of the child object to target

    /* V1
    */
void Explode()
{
    // Ensure there are at least two children in the GameObject
    if (transform.childCount < 2)
    {
        Debug.LogWarning("The GameObject does not have enough children to perform the explode operation!");
        return;
    }

    // Reference the first child (assumed to be Geo_grp)
    Transform geoGroup = transform.GetChild(0);

    // Destroy the second child (assumed to be the bone structure)
    Transform boneStructure = transform.GetChild(1);
    Destroy(boneStructure.gameObject);

    // Play explosion audio if available
    if (explodeAudioClip && audioSource)
    {
        audioSource.PlayOneShot(explodeAudioClip);
    }

    // Detach geoGroup and set it to the root of the scene
    geoGroup.parent = null;

    foreach (Transform child in geoGroup)
    {
        // Check if the current child is "Hand_Left", "Hand_Right", or "Face"
        if (child.name == "Face")
        {
            // Find or create the "Head" GameObject under geoGroup
            Transform headTransform = geoGroup.Find("Head");
            if (headTransform == null)
            {
                GameObject head = new GameObject("Head");
                head.transform.parent = geoGroup;
                head.transform.position = geoGroup.position; // Adjust position if necessary
                headTransform = head.transform;
   
            }
            // Reparent the "Face" object to the "Head" GameObject
            child.parent = headTransform;
        }
        // Check if the current child is "Hand_Left", "Hand_Right", or "Face"
        else if (child.name == "Hand_Left")
        {
           CreateParent(child.name, child, geoGroup) ;
        }
        else if (child.name == "Hand_Right")
        {
           CreateParent(child.name, child, geoGroup);
        } 
        else 
        {

        }
 
    }

    // Iterate over the direct children of Geo_grp
    foreach (Transform child in geoGroup)
    {
        // Assign the "ExplodingObjects" layer to this child
        child.gameObject.layer = LayerMask.NameToLayer("Environment");
     
        // Add Rigidbody if it doesn't already exist
        Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = child.gameObject.AddComponent<Rigidbody>();
            rb.mass = 1f; // Set default mass
            rb.useGravity = true;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            // Apply rotational constraints
            rb.maxAngularVelocity = 5f; // Limit the maximum angular velocity
  
            // Constrain rotation to prevent objects from spinning
            //rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        // Ensure the child has a proper MeshCollider, MeshRenderer, and MeshFilter
        MeshCollider mc = child.gameObject.GetComponent<MeshCollider>();
        MeshFilter mf = child.gameObject.GetComponent<MeshFilter>();
        MeshRenderer mr = child.gameObject.GetComponent<MeshRenderer>();

        SkinnedMeshRenderer smr = child.gameObject.GetComponent<SkinnedMeshRenderer>();
        if (smr != null && smr.sharedMesh != null)
        {
            // Add MeshCollider if missing
            if (mc == null)
            {
                mc = child.gameObject.AddComponent<MeshCollider>();
                mc.sharedMesh = smr.sharedMesh;
                mc.convex = true; // Convex for Rigidbody compatibility
            }

            // Add MeshFilter if missing
            if (mf == null)
            {
                mf = child.gameObject.AddComponent<MeshFilter>();
                mf.sharedMesh = smr.sharedMesh;
            }

            // Add MeshRenderer if missing
            if (mr == null)
            {
                mr = child.gameObject.AddComponent<MeshRenderer>();
                mr.sharedMaterials = smr.sharedMaterials;
            }

            // Remove SkinnedMeshRenderer after transferring its properties
            //Destroy(smr);
        }
        else
        {
            Debug.LogWarning($"No SkinnedMeshRenderer or valid mesh found on {child.gameObject.name}. Collider and renderer may not behave as expected.");
        }

        

        // Apply explosion force with randomized direction
        Vector3 explosionPosition = transform.position + explosionOffset;
        Vector3 randomDirection = (child.position - explosionPosition).normalized + Random.insideUnitSphere * 0.5f; // Add randomness
        float randomForce = explosionForce * Random.Range(0.8f, 1.2f); // Slight variation in force
        rb.AddExplosionForce(randomForce, explosionPosition, explosionRadius, upwardModifier, ForceMode.Impulse);

        // OPTIONAL: Add torque (minimal for slight spinning, can be removed for no spin)
        Vector3 randomTorque = Random.insideUnitSphere * explosionForce * 0.01f; // Reduced torque
        rb.AddTorque(randomTorque, ForceMode.Impulse);

        // Iterate over grandchildren to process their colliders and renderers
        foreach (Transform grandChild in child)
        {
            MeshCollider grandChildCollider = grandChild.gameObject.GetComponent<MeshCollider>();
            MeshFilter grandChildFilter = grandChild.gameObject.GetComponent<MeshFilter>();
            MeshRenderer grandChildRenderer = grandChild.gameObject.GetComponent<MeshRenderer>();

            SkinnedMeshRenderer grandChildSMR = grandChild.gameObject.GetComponent<SkinnedMeshRenderer>();
            if (grandChildSMR != null && grandChildSMR.sharedMesh != null)
            {
                // Add MeshCollider if missing
                if (grandChildCollider == null)
                {
                    grandChildCollider = grandChild.gameObject.AddComponent<MeshCollider>();
                    grandChildCollider.sharedMesh = grandChildSMR.sharedMesh;
                    grandChildCollider.convex = true;
                }

                // Add MeshFilter if missing
                if (grandChildFilter == null)
                {
                    grandChildFilter = grandChild.gameObject.AddComponent<MeshFilter>();
                    grandChildFilter.sharedMesh = grandChildSMR.sharedMesh;
                }

                // Add MeshRenderer if missing
                if (grandChildRenderer == null)
                {
                    grandChildRenderer = grandChild.gameObject.AddComponent<MeshRenderer>();
                    grandChildRenderer.sharedMaterials = grandChildSMR.sharedMaterials;
                }

                // Remove SkinnedMeshRenderer after transferring its properties
                //Destroy(grandChildSMR);
            }
            else
            {
                Debug.LogWarning($"No SkinnedMeshRenderer or valid mesh found on {grandChild.gameObject.name}. Collider and renderer may not behave as expected.");
            }
        }

        // Start fading out and destroy after a delay
        //StartCoroutine(FadeOutAndDestroy(child.gameObject, destructionDelay));

        // Destroy each child after the destruction delay
        Destroy(child.gameObject, destructionDelay);

        
    }

    // Log explosion for debugging
    Debug.Log("Explosion triggered on Geo_grp! Bone structure destroyed. geoGroup set to root.");

    // Destroy the parent GameObject immediately
    Destroy(gameObject);
}


void CreateParent(string name, Transform child, Transform geoGroup)
{
        // Create a new parent GameObject
        GameObject parent = new GameObject($"{name}_Parent");
        parent.transform.position = child.position;
        parent.transform.rotation = child.rotation;

        // Reparent the current child to the new parent
        child.parent = parent.transform;

        // Make the new parent GameObject a child of geoGroup
        parent.transform.parent = geoGroup;
}


IEnumerator FadeOutAndDestroy(GameObject obj, float fadeDuration)
{
    // Retrieve the Renderer to access the materials
    Renderer renderer = obj.GetComponent<Renderer>();
    if (renderer == null)
    {
        Debug.LogWarning($"No Renderer found on {obj.name}. Skipping fade-out.");
        yield break;
    }

    Material[] materials = renderer.materials; // Get all materials (for multi-material objects)
    float elapsedTime = 0f;

    // Store the original colors of the materials
    Color[] originalColors = new Color[materials.Length];
    for (int i = 0; i < materials.Length; i++)
    {
        originalColors[i] = materials[i].color;
    }

    while (elapsedTime < fadeDuration)
    {
        float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration); // Gradually reduce alpha

        // Update materials' alpha
        for (int i = 0; i < materials.Length; i++)
        {
            Color newColor = originalColors[i];
            newColor.a = alpha;
            materials[i].color = newColor;
        }

        elapsedTime += Time.deltaTime;
        yield return null; // Wait for the next frame
    }

    // Ensure alpha is fully 0 and destroy the object
    for (int i = 0; i < materials.Length; i++)
    {
        Color newColor = originalColors[i];
        newColor.a = 0f;
        materials[i].color = newColor;
    }
    Destroy(obj);
}




/// <summary>
/// Adds a BoxCollider to the specified group based on the bounds of its children.
/// </summary>
        void AddBoxColliderToGroup(Transform group)
        {
            // Calculate the bounds of all child renderers
            Bounds bounds = new Bounds(group.position, Vector3.zero);
            Renderer[] renderers = group.GetComponentsInChildren<Renderer>();

            if (renderers.Length == 0)
            {
                Debug.LogWarning("No renderers found in the group to calculate bounds for BoxCollider!");
                return;
            }

            // Encapsulate all child renderers
            foreach (Renderer renderer in renderers)
            {
                bounds.Encapsulate(renderer.bounds);
            }

            // Add a BoxCollider to the group and adjust its size and center
            BoxCollider boxCollider = group.gameObject.AddComponent<BoxCollider>();
            boxCollider.center = group.InverseTransformPoint(bounds.center);
            boxCollider.size = bounds.size;

            Debug.Log("BoxCollider added to geoGroup with calculated bounds.");
        }

 

        protected virtual void Awake()
        {
            minifig = GetComponent<EnemyMinifig>();
            animator = GetComponent<Animator>();
            animators = GetComponentsInChildren<Animator>();
            audioSource = GetComponent<AudioSource>();
  
   
        }

    protected virtual void Update()
            {
                if (exploded)
                {
                    return;
                }

                 if (Input.GetKeyDown(KeyCode.E)) // Change key as needed
                {
                    Explode();
                }
            }


    protected void EnemyHandleExplode()
            {
                
                minifig = gameObject.GetComponent<EnemyMinifig>();
                const float horizontalVelocityTransferRatio = 0.35f;
                const float verticalVelocityTransferRatio = 0.1f;
                const float angularVelocityTransferRatio = 1.0f;

                var transferredSpeed = Vector3.Scale(moveDelta + externalMotion, new Vector3(horizontalVelocityTransferRatio, verticalVelocityTransferRatio, horizontalVelocityTransferRatio));
                var transferredAngularSpeed = (rotateSpeed + externalRotation) * angularVelocityTransferRatio;

                if (explodeAudioClip)
                {
                audioSource.PlayOneShot(explodeAudioClip);
                }

                   // Check for missing joints before exploding
                if (leftArmTip == null || rightArmTip == null || leftLegTip == null || rightLegTip == null || head == null)
                {
                    Debug.LogError("One or more required limb parts are missing for the explosion!");
                    return; // Exit early if joints are missing
                } 
                else 
                {

                Debug.LogError("EnemyHandleExplode called");

                // Explode Minifigure
                EnemyMinifigExploder.Explode(minifig, leftArmTip, rightArmTip, leftLegTip, rightLegTip, head, transferredSpeed, transferredAngularSpeed);
        
                }

            }

            public virtual void EnemyExploder()
            {
                if (!exploded)
                {
                    exploded = true;
                    animator.enabled = false;

                    // Move Minifigure to root hierarchy.
                    gameObject.transform.parent = null;

                    Debug.Log("EnemyExploder Called");
                    
                    EnemyHandleExplode();
                    Destroy(gameObject);
                }
            }

            protected virtual void OnValidate()
            {

                if (!leftArmTip || !rightArmTip || !leftLegTip || !rightLegTip || !head)
                {
                    var rigTransform = transform.GetChild(1);
                    if (rigTransform)
                    {
                        FindJointReferences(rigTransform);
                    }
                    else
                    {
                        Debug.LogError("Failed to find Minifigure rig.");
                    }
                }
            }


            protected void FindJointReferences(Transform parent)
            {
                foreach (Transform child in parent)
                {
                    switch (child.name)
                    {
                        case "wrist_L":
                            leftArmTip = child;
                            break;
                        case "wrist_R":
                            rightArmTip = child;
                            break;
                        case "foot_L":
                            leftLegTip = child;
                            break;
                        case "foot_R":
                            rightLegTip = child;
                            break;
                        case "head":
                            head = child;
                            break;
                    }

                    FindJointReferences(child);
                }
            }
    }
}