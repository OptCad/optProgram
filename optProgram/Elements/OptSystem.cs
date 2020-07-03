using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optProgram.elements
{
    public class OptSystem
    {
        Queue<double> RefractiveIndexQ = new Queue<double>();
        Queue<double> RadiusQ = new Queue<double>();
        Queue<double> IntervalQ = new Queue<double>();
        Queue<double> IncidentAngleQ = new Queue<double>();
        Queue<double> ExitAngleQ = new Queue<double>();
        double pupilDiameter;
        public OptSystem(Queue<Sphere> systemdata, Obj obj,double pupilD)
        {

            this.pupilDiameter = pupilD;

            RefractiveIndexQ.Enqueue(obj.envRefractive);
            int inputCount = systemdata.Count;
            while (systemdata.Count > 0)
            {

                Sphere tmp = systemdata.Dequeue();
                RadiusQ.Enqueue(tmp.r);
                RefractiveIndexQ.Enqueue(tmp.n);

                if(inputCount!=1&&systemdata.Count!=0)
                    IntervalQ.Enqueue(tmp.d);

            }
        }
        public Beam GaussianRefraction(Beam incidentBeam1,bool isinf) //Calculate exit beam using recursion
        {
            double u1p, l1p, u2, l2;
            double incidentAngle, exitAngle;
            double radius = RadiusQ.Dequeue();
            double n = RefractiveIndexQ.Dequeue();
            double np = RefractiveIndexQ.Peek(); //Do not delete np item for later use: n2 = n1p
            if (isinf)
            {
                incidentAngle = pupilDiameter / 2 / radius;
                isinf = !isinf;
            }
            else
            {
                incidentAngle = (incidentBeam1.l - radius) * incidentBeam1.u / radius;
            }
            exitAngle = n * incidentAngle / np;
            IncidentAngleQ.Enqueue(incidentAngle);
            ExitAngleQ.Enqueue(exitAngle);
            u1p = incidentAngle + incidentBeam1.u - exitAngle;
            l1p = radius + radius * exitAngle / u1p;

            Beam exitBeam1 = new Beam(l1p, u1p);
            if (RadiusQ.Count == 0) return exitBeam1; // the last sphere does not have any sphere behind

            u2 = u1p;
            l2 = l1p - IntervalQ.Dequeue();
            Beam incidentBeam2 = new Beam(l2, u2);

            return GaussianRefraction(incidentBeam2,isinf);
        }


        public Beam RealRefraction(Beam incidentBeam1, bool isinf) //Calculate exit beam using recursion
        {

            double u1p, l1p, u2, l2;
            double incidentAngle, exitAngle;
            double radius = RadiusQ.Dequeue();
            double n = RefractiveIndexQ.Dequeue();
            double np = RefractiveIndexQ.Peek(); //Do not delete np item for later use: n2 = n1p

            double sinI;
            if (isinf)
            {
                sinI = pupilDiameter / radius;
                if (Math.Abs(sinI) > 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(sinI), "入射光线超半球！");
                }
                incidentAngle = Math.Asin(sinI);
                isinf = !isinf;
            }
            else
            {
                sinI = (incidentBeam1.l - radius) * Math.Sin(incidentBeam1.u) / radius;
                if(Math.Abs(sinI)>1)
                {
                    throw new ArgumentOutOfRangeException(nameof(sinI), "入射光线超半球！");
                }
                incidentAngle = Math.Asin(sinI);

            }
            // Math.Asin(1) = 1.5707963267948966 Math.Sin(Math.PI) = 0
            // using rad not deg                       
            double sinIp = n * sinI / np;
            if (Math.Abs(sinIp) > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(sinIp), "发生全反射！");
            }
            exitAngle = Math.Asin(sinIp);

            IncidentAngleQ.Enqueue(incidentAngle);
            ExitAngleQ.Enqueue(exitAngle);

            u1p = incidentAngle + incidentBeam1.u - exitAngle;
            l1p = radius + radius * sinIp / Math.Sin(u1p);

            Beam exitBeam1 = new Beam(l1p, u1p);
            if (RadiusQ.Count == 0) return exitBeam1; // the last sphere does not have any sphere behind

            u2 = u1p;
            l2 = l1p - IntervalQ.Dequeue();
            Beam incidentBeam2 = new Beam(l2, u2);

            return RealRefraction(incidentBeam2,isinf);
        }


    }
}
