/*
 *  WaypointFollower.cs
 *
 *  Makes a GameObject follow a path marked 
 *  by waypoints.  Will also chase a 
 *  specified object if it comes within range
 *  (and if it can see that object).
 *
 *  Attach this script to a GameObject.  
 *  Then drag other GameObjects into the 
 *  waypoint list.
 *
 *  The GameObject will move from waypoint
 *  to waypoint, in order.
 *
 *  WalkingSpeed
 *      in metres per second.
 *      
 *  TurningSpeed
 *      in degrees per second.
 *      
 *  RepeatForever
 *      repeat the path forever.
 *      
 *  PingPong
 *      At the end, go backwards through
 *      the waypoints.
 *      
 *  Chase Object
 *      GameObject that will be chased ...
 *      
 *  Chase Range
 *      ... if it comes within this range...
 *      
 *  Chase Angle
 *      ...and angle of view.
 *
 *  Chase Min Closeness
 *      Stop this distance away from the
 *      chased object.
 *  
 *  Chase Hud Alert
 *      This object is enabled when chasing.
 *      Ideal for a HUD warning, for example
 *      "You've been seen!"
 *      
 *  Chase Obey Walls
 *      Sets whether the chase is blocked by
 *      obstacles.
 *  
 *  v2.00 -- Added to JKScriptPack2
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKScriptPack2
{

    public class WaypointFollower : MonoBehaviour
    {

        public GameObject[] waypointList = new GameObject[1];
        public float walkingSpeed = 4;          // distance per second
        public float turningSpeed = 90;         // degrees per second
        public bool repeatForever = true;
        public bool pingPong = false;           // reverse path at end?
        public bool glide = false;              // do we turn and walk at the same time?

        private float autocorrectDistance = 0.1f;
        private float autocorrectAngle = 1.0f;

        private GameObject target;
        private int index = 0;
        private int increment = 1;              // Switches to -1 if reversing

        [System.Serializable]
        public class Chase
        {
            public GameObject chaseObject;
            public float range = 10.0f;
            public float angle = 90;
            public float minCloseness = 2.0f;
            public GameObject hudAlert;
            public bool obeyWalls = false;
        }
        public Chase chase;
        
        [System.Serializable]
        public class AnimatedCharacter
        {
            public string idle;
            public string walk;
            public bool idleWhenTurning = false;
        }
        public AnimatedCharacter animmode;
        private Animation anim;

        public bool onFlatPlane = true;

        void Start()
        {
            if (chase.hudAlert) chase.hudAlert.SetActive(false);
            anim = GetComponentInChildren<Animation>();
            if (anim) anim.Play(animmode.idle, PlayMode.StopAll);

            // Remove this and the chase object from raycasts
            chase.chaseObject.layer = 2;
            SetLayerIncludingChildren(this.gameObject, 2);
        }

        void Update()
        {

            // Where am I?
            Vector3 currentPos = this.transform.position;
            Vector3 currentHeading = this.transform.forward;

            // Presume we're heading to the current waypoint
            target = CurrentWaypoint();
            Debug.DrawRay(currentPos, target.transform.position - currentPos, Color.cyan);

            // Consider the chase object
            if (chase.chaseObject && chase.angle > 0 && chase.range > 0)
            {

                // Draw the chase zone
                float halfangle = chase.angle / 2;
                Vector3 lastpos = Vector3.zero;
                for (float a = -halfangle; a <= halfangle; a += halfangle / 12)
                {
                    Vector3 now = (Quaternion.AngleAxis(a, Vector3.up) * currentHeading) * chase.range;
                    Debug.DrawLine(currentPos + lastpos, currentPos + now, Color.yellow);
                    lastpos = now;
                }
                Debug.DrawLine(currentPos + lastpos, currentPos, Color.yellow);

                // Where is it?
                Vector3 victimPos = chase.chaseObject.transform.position;
                Vector3 victimHeading = victimPos - currentPos;

                // Check range & visibility
                float victimDistance = victimHeading.magnitude;
                float victimAngle = Vector3.Angle(currentHeading, victimHeading);
                if (victimDistance <= chase.range && victimAngle <= (chase.angle / 2))
                {
                    bool viewBlocked = false;
                    if (chase.obeyWalls)
                    {

                        // Check whether the view is blocked
                        RaycastHit hitinfo;
                        viewBlocked = Physics.Raycast(currentPos, victimHeading.normalized, out hitinfo, victimDistance);

                        // Draw ray to blockage
                        if (viewBlocked)
                        {
                            float hitDistance = Vector3.Distance(currentPos, hitinfo.collider.transform.position);
                            Debug.DrawRay(currentPos, victimHeading.normalized * hitDistance, Color.red);
                        }

                    }
                    if (!viewBlocked)
                    {
                        // Draw ray to chase object
                        target = chase.chaseObject;
                        Debug.DrawRay(currentPos, victimHeading, Color.green);
                    }
                }

                // Update HUD
                if (chase.hudAlert)
                {
                    chase.hudAlert.SetActive(target == chase.chaseObject);
                }

            }

            // If we have a target... (will be null if the waypointlist is empty and we're not chasing a victim)
            if (target)
            {

                // Where is it?
                Vector3 targetPos;
                if (onFlatPlane)
                {
                    targetPos = new Vector3(target.transform.position.x, currentPos.y, target.transform.position.z);
                }
                else
                {
                    targetPos = target.transform.position;
                }
                Vector3 targetHeading = targetPos - currentPos;

                // Have we reached the target?
                if (targetHeading.magnitude < autocorrectDistance)
                {

                    // Move on to next target
                    transform.position = targetPos;
                    if (target != chase.chaseObject)
                    {
                        target = NextWaypoint();
                    }

                }
                else
                {

                    // Do we need to turn?
                    if (Vector3.Angle(currentHeading, targetHeading) > autocorrectAngle)
                    {

                        // Turn towards the target
                        Vector3 newDirection = Vector3.RotateTowards(currentHeading, targetHeading, turningSpeed * Mathf.Deg2Rad * Time.deltaTime, 0.0f);
                        transform.rotation = Quaternion.LookRotation(newDirection);

                        if (animmode.idleWhenTurning)
                        {
                            AnimateIdle();
                        }

                    }
                    else
                    {

                        // Face the target
                        transform.rotation = Quaternion.LookRotation(targetHeading);

                    }

                    // If we're facing the object (or if glide is enabled) then walk
                    if (glide || Vector3.Angle(currentHeading, targetHeading) < autocorrectAngle)
                    {

                        if (chase.chaseObject && target == chase.chaseObject && targetHeading.magnitude <= chase.minCloseness)
                        {
                            // Do nothing
                            AnimateIdle();
                        }
                        else
                        {

                            // Walk toward the target
                            transform.position = Vector3.MoveTowards(currentPos, targetPos, walkingSpeed * Time.deltaTime);
                            AnimateWalk();

                        }
                    }

                }
            }

        }

        private void SetLayerIncludingChildren(GameObject g, int layer)
        {
            if (g == null)
            {
                return;
            }
            g.layer = layer;
            foreach (Transform t in g.transform)
            {
                if (t != null)
                {
                    SetLayerIncludingChildren(t.gameObject, layer);
                }
            }
        }

        private void AnimateIdle()
        {
            if (anim && !anim.IsPlaying(animmode.idle))
            {
                anim.CrossFade(animmode.idle);
            }
        }

        private void AnimateWalk()
        {
            if (anim && !anim.IsPlaying(animmode.walk))
            {
                anim.CrossFade(animmode.walk);
            }
        }

        private GameObject CurrentWaypoint()
        {
            if (waypointList.Length <= 0)
            {
                return null;
            }
            if (index < 0)
            {
                index = 0;
            }
            if (index >= waypointList.Length)
            {
                index = waypointList.Length - 1;
            }
            return (waypointList[index]);
        }

        private GameObject NextWaypoint()
        {
            if (waypointList.Length <= 0)
            {
                return null;
            }
            index += increment;
            if (index >= waypointList.Length)
            {
                if (pingPong)
                {
                    increment = -1;
                    index = waypointList.Length - 1;
                }
                else if (repeatForever)
                {
                    index = 0;
                }
                else
                {
                    index = waypointList.Length - 1;
                    increment = 0;
                }
            }
            if (index < 0)
            {
                if (repeatForever)
                {
                    increment = 1;
                }
                else
                {
                    increment = 0;
                }
                index = 0;
            }
            return waypointList[index];
        }

    }

}
