namespace Bezier
{
    /**
     * An enumrator that is used to 
     * describe the way the bezier points 
     * meet
     */
    [System.Serializable]
    public enum BezierControlPointMode {
        Free,
        Aligned,
        Mirrored
    }
}