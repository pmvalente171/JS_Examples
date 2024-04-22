using Bezier.Spline;

namespace Bezier
{
    public interface IDefaultWalker 
    {
    /**
       * This method verifys if
       * the progress of the ship is at
       * one, or zero depending one the direction
       * the object is traveling, if it is
       * the method resets the progress 
       * of the ship
       */
    void setProgressAtOne();

    /**
       * this method is used to 
       * update the progress value
       * of the ship every frame
       */
    void updateProgress();

    /**
       * This method updates 
       * the current position 
       * of the walker
       */
    void updatePosition();

    /**
       * This getter returns 
       * the value of 
       * the current ship progress
       */
    float Progress { get; }

    /**
       * This method is used to 
       * set the value of the walkers 
       * spline 
       */
    BezierSpline Spline { set; }
    }
}
