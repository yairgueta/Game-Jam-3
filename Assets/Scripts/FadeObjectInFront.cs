using UnityEngine;

public class FadeObjectInFront : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.position.y > transform.position.y) return;
        var hit = other.gameObject.GetComponent<FadingObject>();
        if (hit == null) return;
        if (hit.HasFaded) return;
        hit.Fade();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.position.y > transform.position.y) return;
        var hit = other.gameObject.GetComponent<FadingObject>();
        if (hit == null) return;
        if (hit.HasFaded) return;
        hit.Fade();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var hit = other.gameObject.GetComponent<FadingObject>();
        if (hit == null) return;
        hit.Unfade();
    }
}
