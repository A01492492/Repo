function Start () {
    // Create a new alpha-only texture and assign it
    // to the renderer's material
    var texture = new Texture2D (128, 128, TextureFormat.Alpha8, false);
    renderer.material.mainTexture = texture;
}