using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Selected : MonoBehaviour
{

    LayerMask mask;
    [SerializeField] float distance = 1.5f;

    [SerializeField] Texture2D puntero;
    [SerializeField] GameObject TextDetect;
    GameObject lastRecogniced = null;

    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("Raycast Detect");
        TextDetect.GetComponent<TextMeshProUGUI>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Raycast(origen, direccion, objeto de impacto, distancia, m�scara)
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distance, mask))
        {
            Deselect();
            if (hit.collider.tag == "IntObj")
            {
                SelectedObject(hit.collider.transform);

                if (Input.GetMouseButtonDown(1))
                {
                    InteractableObject hitInt = hit.collider.transform.GetComponent<InteractableObject>();
                    if(hitInt != null){
                        hitInt.OnInteract(gameObject.transform.parent.gameObject);
                    }
                }
            }
        }
        else
        {
            Deselect();
        }
    }

    void SelectedObject(Transform t)
    {
        var exist = t.TryGetComponent<Outline>(out var outline);
        if(exist)
        {
            outline.OutlineWidth = 10f;
        }
        lastRecogniced = t.gameObject;
    }

    void Deselect()
    {
        if (lastRecogniced)
        {
            lastRecogniced.GetComponent<Outline>().OutlineWidth = 0f;
            lastRecogniced = null;
        }
    }
    private void OnGUI()
    {
        Rect rect = new Rect(Screen.width / 2, Screen.height / 2, puntero.width, puntero.height);
        GUI.DrawTexture(rect, puntero);

        if (lastRecogniced)
        {
            TextDetect.GetComponent<TextMeshProUGUI>().enabled = true;
        } else
        {
            TextDetect.GetComponent<TextMeshProUGUI>().enabled = false;
        }
    }
}
