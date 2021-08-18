using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Helper;


public class SpawManager : MonoBehaviour
{
    [SerializeField] private GameObject SuspectPrefab;
    [SerializeField] private GameObject VictimPrefab;
    [SerializeField] private GameObject Player;
    [SerializeField] private float RespawnRateMin = 1;
    [SerializeField] private float RespawnRateMax = 2;

    private List<Transform> _children;
    private Dictionary<Transform, Transform[]> _neighborSpawners = new Dictionary<Transform, Transform[]>();

    private void Start()
    {
        _children = transform.Children();

        _neighborSpawners[_children[0]] = new Transform[1] { _children[1] };
        _neighborSpawners[_children[1]] = new Transform[2] { _children[0], _children[2] };
        _neighborSpawners[_children[2]] = new Transform[2] { _children[1], _children[3] };
        _neighborSpawners[_children[3]] = new Transform[1] { _children[2] };

        StartCoroutine(SpawnCharacter());
    }

    private IEnumerator SpawnCharacter()
    {
        while (true)
        {
            float respawnTime = UnityEngine.Random.Range(RespawnRateMin, RespawnRateMax);
            yield return new WaitForSecondsRealtime(respawnTime);
            (Transform, Transform) path = GenerateRandomPath();
            GameObject character = InstantiateRandomCharacter(path);
            character.transform.position = path.Item1.position;
            character.transform.DOMove(path.Item2.position, 3);

            yield return new WaitForSecondsRealtime(3);
            GameObject.Destroy(character);
        }
    }

    private (Transform, Transform) GenerateRandomPath()
    {
        System.Random r = new System.Random();
        Transform startPos = _children[r.Next(4)];
        Transform stopPos = _neighborSpawners[startPos][r.Next(_neighborSpawners[startPos].GetUpperBound(0))];
        return (startPos, stopPos);
    }
    private GameObject InstantiateRandomCharacter((Transform, Transform) path)
    {
        bool RunningLeft = path.Item1.transform.position.x < path.Item2.transform.position.x;
        GameObject result;
        if (Random.Range(0, 100) < 30)
        {
            // 30% possibility spawn Victim
            result = GameObject.Instantiate(VictimPrefab, Vector3.zero, Quaternion.identity);
            result.GetComponent<Animator>().Play("Run");
            result.transform.LookAt(path.Item2.position - path.Item1.position);
        }
        else
        {
            // 70% possibility spawn Suspect
            result = GameObject.Instantiate(SuspectPrefab, Vector3.zero, Quaternion.identity);
            result.transform.LookAt(Player.transform);
            if (RunningLeft)
                result.GetComponent<Animator>().Play("PistolStrafeLeft");
            else
                result.GetComponent<Animator>().Play("PistolStrafeRight");

        }
        return result;
    }
}
