using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//script untuk mengontrol/menggerakkan bola

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{

    public static BallController instance;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject areaAffector;
    [SerializeField] private float maxForce, forceModifier;
    [SerializeField] private LayerMask rayLayer;

    public GameObject spark;

    private float force;
    private Rigidbody rgBody;

    private Vector3 startPos, endPos;
    private bool canShoot = false, isBallStatic = true;
    private Vector3 direction;


    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        rgBody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //set camera
        CameraFollow.instance.SetTarget(gameObject);
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Hole")
        {
            Instantiate(spark, transform.position, Quaternion.identity);
            //Level Complete from LevelManager
            LevelManager.instance.LevelComplete();
        }
        else if (other.name == "Destroyer")
        {
            //Level Failed
            LevelManager.instance.LevelFailed();

        }
    }

    public void MouseDownMethod()
    {
        if(!isBallStatic) return;

        startPos = ClickedPoint();
        lineRenderer.gameObject.SetActive(true);
        lineRenderer.SetPosition(0, lineRenderer.transform.localPosition);
    }

    public void MouseNormalMethod()
    {
        if (!isBallStatic) return;

        endPos = ClickedPoint();
        endPos.y = lineRenderer.transform.position.y;
        force = Mathf.Clamp(Vector3.Distance(endPos, startPos) * forceModifier, 0, maxForce);
        //show force in UI
        UIManager.instance.PowerImage.fillAmount = force / maxForce;
        lineRenderer.SetPosition(1, transform.InverseTransformPoint(endPos));
    }

    public void MouseUpMethod()
    {
        if (!isBallStatic) return;

        canShoot = true;
        lineRenderer.gameObject.SetActive(false);

        
    }

    private void Update()
    {
        if(rgBody.velocity == Vector3.zero && !isBallStatic)
        {
            isBallStatic = true;
            rgBody.angularVelocity = Vector3.zero;
            areaAffector.SetActive(true);
            LevelManager.instance.ShotTaken();
        }
    }

    private void FixedUpdate()
    {
        if (canShoot)
        {
            canShoot = false;
            direction = startPos - endPos;
            rgBody.AddForce(direction * force, ForceMode.Impulse);
            areaAffector.SetActive(false);
            force = 0;
            startPos = endPos = Vector3.zero;
            isBallStatic = false;
            UIManager.instance.PowerImage.fillAmount = 0;
        }
    }

    Vector3 ClickedPoint()
    {
        Vector3 position = Vector3.zero;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, rayLayer))
        {
            position = hit.point;
        }
        return position;
    }
}
