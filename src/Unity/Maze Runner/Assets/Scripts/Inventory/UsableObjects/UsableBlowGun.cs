using UnityEngine;

public class UsableBlowGun : UsableObject
{
    [SerializeField] GameObject bullet;

    [SerializeField] float bulletSpeed; 

    public override void Use(GameObject player)
    {
        Vector3 forward = player.transform.forward;
        GameObject bul = Instantiate(bullet, player.transform.position + forward, player.transform.rotation);
        bul.GetComponent<Bullet>().SetTeam(player.layer);
        bul.GetComponent<Rigidbody>().AddForce(forward * bulletSpeed, ForceMode.Impulse);
    }
}
