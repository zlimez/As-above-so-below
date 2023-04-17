using UnityEngine;

public class GrabbableSprite : Grabbable
{
    public Sprite notGrabbedSprite;
    public Sprite grabbedSprite;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    public override void OnGrab()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = grabbedSprite;
    }

    public override void OnUngrab()
    {
        spriteRenderer.sprite = notGrabbedSprite;
    }
}
