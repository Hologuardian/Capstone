public struct SimpleVertex
{
    SimpleVertex(float x, float y, float z, byte r, byte g, byte b, byte a)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public float x;
    public float y;
    public float z;
    public byte r;
    public byte g;
    public byte b;
    public byte a;
}
