using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        Obj obj;
        bool isInfinite;
        public OptSystem(Queue<Sphere> systemdata, Obj obj, bool isInfinite)
        {
            this.isInfinite = isInfinite;
            this.obj = obj;
            this.pupilDiameter = obj.pupilDiameter;
            this.RefractiveIndexQ.Enqueue(obj.envRefractive);
            int inputCount = systemdata.Count;
            while (systemdata.Count > 0)
            {

                Sphere tmp = systemdata.Dequeue();
                RadiusQ.Enqueue(tmp.r);
                RefractiveIndexQ.Enqueue(tmp.n);

                if (inputCount != 1 && systemdata.Count != 0)
                    IntervalQ.Enqueue(tmp.d);
            }
        }

        public void calAll()
        {
            double[] K1 = new double[] { 1, 0.85, 0.707, 0.5, 0.3 };
            double[] K2 = new double[] { 1, 0.85, 0.707, 0.5, 0.3, 0, -1, -0.85, -0.707, -0.5, -0.3 };
            Beam incidentBeamGaussian;
            Dictionary<double[], Beam> incidentBeamReal;
            double up_tmp;
            if (obj.objHeight != 0 || obj.fieldAngle != 0)
            {
                incidentBeamGaussian = initOffaxialGaussian();
                incidentBeamReal = initOffAxialReal(K1, K2);
            }
            else
            { 
                incidentBeamGaussian = initOnaxialGaussian();
                incidentBeamReal = initOnAxialReal(K2);
            }


            Beam outputGaussian = GaussianRefraction(incidentBeamGaussian);
            //MessageBox.Show("l:" + outputGaussian.l.ToString() + "\nu:" + outputGaussian.u.ToString());
            Dictionary<double[], Beam> outputReal=new Dictionary<double[], Beam> { };
            foreach (KeyValuePair<double[], Beam> kvp in incidentBeamReal)
            {
                outputReal.Add(kvp.Key, RealRefraction(kvp.Value));
            }
            /*Beam outputReal = RealRefraction(incidentBeam, isInfinite);
            MessageBox.Show("l:" + outputReal.l.ToString() + "\nu:" + outputReal.u.ToString());*/
        }

        private Beam GaussianRefraction(Beam incidentBeam1) //Calculate exit beam using recursion
        {
            // Recurse one more time
            if (RadiusQ.Count == 0)
                return incidentBeam1;
            double u1p, l1p, u2, l2;
            double incidentAngle, exitAngle;
            double radius = RadiusQ.Dequeue();
            double n = RefractiveIndexQ.Dequeue();
            double np = RefractiveIndexQ.Peek(); //Do not delete np item for later use: n2 = n1p
            incidentAngle = (incidentBeam1.l - radius) * incidentBeam1.u / radius;
            exitAngle = n * incidentAngle / np;
            IncidentAngleQ.Enqueue(incidentAngle);
            ExitAngleQ.Enqueue(exitAngle);
            u1p = incidentAngle + incidentBeam1.u - exitAngle;
            l1p = radius + radius * exitAngle / u1p;

            Beam exitBeam1 = new Beam(l1p, u1p);
            //if (RadiusQ.Count == 0) return exitBeam1; the last sphere does not have any sphere behind

            u2 = u1p;
            l2 = l1p - IntervalQ.Dequeue();
            Beam incidentBeam2 = new Beam(l2, u2);

            return GaussianRefraction(incidentBeam2);
        }


        private Beam RealRefraction(Beam incidentBeam1) //Calculate exit beam using recursion
        {

            double u1p, l1p, u2, l2;
            double incidentAngle, exitAngle;
            double radius = RadiusQ.Dequeue();
            double n = RefractiveIndexQ.Dequeue();
            double np = RefractiveIndexQ.Peek(); //Do not delete np item for later use: n2 = n1p

            double sinI;
            sinI = (incidentBeam1.l - radius) * Math.Sin(incidentBeam1.u) / radius;
            if (Math.Abs(sinI) > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(sinI), "入射光线超半球！");
            }
            incidentAngle = Math.Asin(sinI);
            /*
                        if (isinf)
                        {
                            sinI = pupilDiameter / 2 / radius;
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
                            if (Math.Abs(sinI) > 1)
                            {
                                throw new ArgumentOutOfRangeException(nameof(sinI), "入射光线超半球！");
                            }
                            incidentAngle = Math.Asin(sinI);

                        }*/
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

            return RealRefraction(incidentBeam2);
        }

        private Beam initOnaxialGaussian()
        {
            Beam beam;
            if (isInfinite == true)
            {
                double n_tmp = RefractiveIndexQ.Dequeue();
                double np_tmp = RefractiveIndexQ.Peek();
                double r_tmp = RadiusQ.Dequeue();
                double fp = np_tmp / (np_tmp - n_tmp) * r_tmp;
                double lp_tmp;
                if (IntervalQ.Count != 0)
                    lp_tmp = fp - IntervalQ.Dequeue();
                else
                    lp_tmp = fp;
                beam = new Beam(lp_tmp, pupilDiameter / 2 / fp);
            }
            else
                beam = new Beam(obj.objDistance, Math.Sin(obj.apertureAngle));

            return beam;
        }
        private Beam initOffaxialGaussian()
        {
            Beam beam;
            if (isInfinite == true)
                beam = new Beam(0, Math.Sin(obj.fieldAngle));
            else
                beam = new Beam(0, Math.Sin(Math.Atan(obj.objHeight / obj.objDistance)));
            return beam;
        }

        private Dictionary<double[], Beam> initOnAxialReal(double[] K2s)
        {
            Dictionary<double[], Beam> beam = new Dictionary<double[], Beam> { };

            foreach (double K2 in K2s)
            {
                if (isInfinite == true)
                {
                    double n_tmp = RefractiveIndexQ.Dequeue();
                    double np_tmp = RefractiveIndexQ.Peek();
                    double r_tmp = RadiusQ.Dequeue();
                    double fp = np_tmp / (np_tmp - n_tmp) * r_tmp;
                    double lp_tmp;
                    if (IntervalQ.Count != 0)
                        lp_tmp = fp - IntervalQ.Dequeue();
                    else
                        lp_tmp = fp;
                    beam.Add(new double[] { 2,K2 }, new Beam(lp_tmp, Math.Sin(Math.Atan(K2 * pupilDiameter / 2 / fp))));
                }
                else
                    beam.Add(new double[] { 2,K2 }, new Beam(obj.objDistance, K2 * Math.Sin(obj.apertureAngle)));
            }
            return beam;
        }

        private Dictionary<double[], Beam> initOffAxialReal(double[] K1s, double[] K2s)
        {
            Dictionary<double[], Beam> beam = new Dictionary<double[], Beam> { };
            foreach (double K1 in K1s)
            {
                foreach (double K2 in K2s)
                {
                    if (isInfinite == true)
                        beam.Add(new double[] { K1, K2 }, new Beam(K2 * pupilDiameter / 2 / Math.Tan(K1 * obj.fieldAngle), K1 * obj.fieldAngle));
                    else
                    {
                        double tanU1;
                        tanU1 = (K1 * obj.objHeight - K2 * pupilDiameter / 2) / obj.objDistance;
                        beam.Add(new double[] { K1, K2 }, new Beam(K2 * pupilDiameter / 2 / tanU1, Math.Atan(tanU1)));
                    }
                }
            }

            return beam;
        }
    }
}
