using UnityEngine;

public class CameraTargetEnd : MonoBehaviour
{
    public Transform start, end, obj;
    private Player player;
    private Vector3 minVec;

    void Start()
    {
        player = Player.instance;
    }

    void Update()
    {
        minVec = obj.position;

        Vector2 vec0 = new Vector2(end.position.x - start.position.x, end.position.z - start.position.z);
        Vector2 vec = vec0.normalized;
        float bh = ((player.transform.position.x-start.position.x)*vec.x + (player.transform.position.z-start.position.z)*vec.y)/Mathf.Sqrt(vec.x*vec.x + vec.y*vec.y);
        
        Vector3 vecGlobal = start.position + ((end.position-start.position)* (bh / vec0.magnitude));
        
        obj.position = new Vector3(Mathf.Max(vecGlobal.x,minVec.x), 0, Mathf.Max(vecGlobal.z,minVec.z));

    }
    
}
