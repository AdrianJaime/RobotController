using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotController
{

    public struct MyQuat
    {

        public float w;
        public float x;
        public float y;
        public float z;

        //Constructor
        public MyQuat(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }

    public struct MyVec
    {

        public float x;
        public float y;
        public float z;

        //Constructor
        public MyVec(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }






    public class MyRobotController
    {

        #region public methods



        public string Hi()
        {

            string s = "Dll??";
            return s;

        }


        //EX1: this function will place the robot in the initial position

        public void PutRobotStraight(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3) {

            //Restart exercises
            finished = false;

            //Set first quaternion to identity to keep it at no rotation
            rot0 = Rotate(new MyQuat(0, 0, 0, 1), new MyVec(0, 1, 0), 73);
            
            //Rotate childs
            rot1 = Multiply(rot0, Rotate(new MyQuat(0, 0, 0, 1), new MyVec(1, 0, 0), startingPoint[1]));
            rot2 = Multiply(rot1, Rotate(new MyQuat(0, 0, 0, 1), new MyVec(1, 0, 0), startingPoint[2]));
            rot3 = Multiply(rot2, Rotate(new MyQuat(0, 0, 0, 1), new MyVec(1, 0, 0), startingPoint[3]));
        }

        

        //EX2: this function will interpolate the rotations necessary to move the arm of the robot until its end effector collides with the target (called Stud_target)
        //it will return true until it has reached its destination. The main project is set up in such a way that when the function returns false, the object will be droped and fall following gravity.


        public bool PickStudAnim(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3)
        {
            //First time it goes into this function it sets everything up.
            if (!myCondition && !finished)
            {
                lerpValue = 0;
                myCondition = true;
            }


            //lerp between the startingPoint and the endingPoint to know the rotation value we'll have to multiply by.
            if(myCondition)
            {
                lerpValue += 0.01f;
                float rotation0 = Lerp(startingPoint[0], endingPoint[0], lerpValue);
                float rotation1 = Lerp(startingPoint[1], endingPoint[1], lerpValue);
                float rotation2 = Lerp(startingPoint[2], endingPoint[2], lerpValue);
                float rotation3 = Lerp(startingPoint[3], endingPoint[3], lerpValue);


                //Check if finished
                if (rotation0 <= endingPoint[0] && rotation1 >= endingPoint[1] && rotation2 <= endingPoint[2] && rotation3 <= endingPoint[3])
                { 
                    myCondition = false;
                    finished = true;
                }

                rot0 = Rotate(Rotate(new MyQuat(0, 0, 0, 1), new MyVec(0, 1, 0), 73f), new MyVec(0, 1, 0), rotation0);
                rot1 = Multiply(rot0, Rotate(new MyQuat(0, 0, 0, 1), new MyVec(1, 0, 0), rotation1));
                rot2 = Multiply(rot1, Rotate(new MyQuat(0, 0, 0, 1), new MyVec(1, 0, 0), rotation2));
                rot3 = Multiply(rot2, Rotate(new MyQuat(0, 0, 0, 1), new MyVec(1, 0, 0), rotation3));

                return true;
            }
            //todo: remove this once your code works.
            rot0 = NullQ;
            rot1 = NullQ;
            rot2 = NullQ;
            rot3 = NullQ;

            return false;
        }


        //EX3: this function will calculate the rotations necessary to move the arm of the robot until its end effector collides with the target (called Stud_target)
        //it will return true until it has reached its destination. The main project is set up in such a way that when the function returns false, the object will be droped and fall following gravity.
        //the only difference wtih exercise 2 is that rot3 has a swing and a twist, where the swing will apply to joint3 and the twist to joint4

        public bool PickStudAnimVertical(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3)
        {

            bool myCondition = false;
            //todo: add a check for your condition



            while (myCondition)
            {
                //todo: add your code here


            }

            //todo: remove this once your code works.
            rot0 = NullQ;
            rot1 = NullQ;
            rot2 = NullQ;
            rot3 = NullQ;

            return false;
        }


        public static MyQuat GetSwing(MyQuat rot3)
        {
            //todo: change the return value for exercise 3
            return NullQ;

        }


        public static MyQuat GetTwist(MyQuat rot3)
        {
            //todo: change the return value for exercise 3
            return NullQ;

        }




        #endregion


        #region private and internal methods

        internal int TimeSinceMidnight { get { return (DateTime.Now.Hour * 3600000) + (DateTime.Now.Minute * 60000) + (DateTime.Now.Second * 1000) + DateTime.Now.Millisecond; } }

        bool myCondition = false;
        bool finished = false;

        float lerpValue = 0;

        float[] startingPoint = { 0, -5, 87, 24 };

        float[] endingPoint = { -32, 17, 57, 20 };

        private static MyQuat NullQ
        {
            get
            {
                MyQuat a;
                a.w = 1;
                a.x = 0;
                a.y = 0;
                a.z = 0;
                return a;

            }
        }

        internal MyQuat Multiply(MyQuat q1, MyQuat q2) {

            //todo: change this so it returns a multiplication:
            MyQuat myQuaternion = new MyQuat();
            myQuaternion.w = (float)((double)q2.w * (double)q1.w - (double)q2.x * (double)q1.x - (double)q2.y * (double)q1.y - (double)q2.z * (double)q1.z);
            myQuaternion.x = (float)((double)q2.w * (double)q1.x + (double)q2.x * (double)q1.w - (double)q2.y * (double)q1.z + (double)q2.z * (double)q1.y);
            myQuaternion.y = (float)((double)q2.w * (double)q1.y + (double)q2.x * (double)q1.z + (double)q2.y * (double)q1.w - (double)q2.z * (double)q1.x);
            myQuaternion.z = (float)((double)q2.w * (double)q1.z - (double)q2.x * (double)q1.y + (double)q2.y * (double)q1.x + (double)q2.z * (double)q1.w);
            myQuaternion = Normalize(myQuaternion);
            return myQuaternion;
        }

        internal MyQuat Rotate(MyQuat currentRotation, MyVec axis, float angle)
        {

            //todo: change this so it takes currentRotation, and calculate a new quaternion rotated by an angle "angle" radians along the normalized axis "axis"
            MyQuat myQuaternion = new MyQuat();

            angle = angle * (float)(Math.PI / 180);

            float num = (float)Math.Sqrt(Math.Pow((double)axis.x, 2.0) + Math.Pow((double)axis.y, 2.0) + Math.Pow((double)axis.z, 2.0));
            axis.x /= num;
            axis.y /= num;
            axis.z /= num;

            myQuaternion.x = axis.x * (float)Math.Sin((double)angle / 2.0);
            myQuaternion.y = axis.y * (float)Math.Sin((double)angle / 2.0);
            myQuaternion.z = axis.z * (float)Math.Sin((double)angle / 2.0);
            myQuaternion.w = (float)Math.Cos((double)angle / 2.0);
            myQuaternion = Normalize(myQuaternion);


            currentRotation = Normalize(currentRotation);

            return Multiply(myQuaternion, currentRotation);
        }




        //todo: add here all the functions needed

        internal MyQuat Normalize(MyQuat myQuaternion)
        {
            double result = Math.Sqrt((double)myQuaternion.x * (double)myQuaternion.x + (double)myQuaternion.y * (double)myQuaternion.y + (double)myQuaternion.z * (double)myQuaternion.z + (double)myQuaternion.w * (double)myQuaternion.w);

            myQuaternion.x /= (float)result;
            myQuaternion.y /= (float)result;
            myQuaternion.z /= (float)result;
            myQuaternion.w /= (float)result;

            return myQuaternion;
        }

        internal float Lerp(float initialFloat, float finalFloat, float f)
        {
            return initialFloat + f * (finalFloat - initialFloat);
        }
        #endregion






    }
}
