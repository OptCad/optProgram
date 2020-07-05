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
        Dictionary<string, Beam> outputReal = new Dictionary<string, Beam> { };
        Beam outputGaussian;
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
            double[] K1 = new double[] { 1, 0.85, 0.7, 0.5, 0.3 };
            double[] K2 = new double[] { 1, 0.85, 0.7, 0.5, 0.3, 0, -1, -0.85, -0.7, -0.5, -0.3 };
            Beam incidentBeamGaussian;
            Dictionary<string, Beam> incidentBeamReal;
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

            outputGaussian = GaussianRefraction(incidentBeamGaussian, new Queue<double>(RadiusQ), new Queue<double>(RefractiveIndexQ), new Queue<double>(IntervalQ));

            MessageBox.Show("l:" + outputGaussian.l.ToString() + "\nu:" + outputGaussian.u.ToString());

            foreach (KeyValuePair<string, Beam> kvp in incidentBeamReal)
            {
                outputReal.Add(kvp.Key, RealRefraction(kvp.Value, new Queue<double>(RadiusQ), new Queue<double>(RefractiveIndexQ), new Queue<double>(IntervalQ)));
            }
            /*Beam outputReal = RealRefraction(incidentBeam, isInfinite);
            MessageBox.Show("l:" + outputReal.l.ToString() + "\nu:" + outputReal.u.ToString());*/
            sphericalAber(outputReal, outputGaussian);
        }

        private Beam GaussianRefraction(Beam incidentBeam1, Queue<double> Radius,
            Queue<double> RefractiveIndex, Queue<double> Interval) //Calculate exit beam using recursion
        {

            if (Radius.Count == 0)
                return incidentBeam1;
            double u1p, l1p, u2, l2;
            double incidentAngle, exitAngle;
            double radius = Radius.Dequeue();
            double n = RefractiveIndex.Dequeue();
            double np = RefractiveIndex.Peek(); //Do not delete np item for later use: n2 = n1p
            incidentAngle = (incidentBeam1.l - radius) * incidentBeam1.u / radius;
            exitAngle = n * incidentAngle / np;
            IncidentAngleQ.Enqueue(incidentAngle);
            ExitAngleQ.Enqueue(exitAngle);
            u1p = incidentAngle + incidentBeam1.u - exitAngle;
            l1p = radius + radius * exitAngle / u1p;

            Beam exitBeam1 = new Beam(l1p, u1p);
            if (Radius.Count == 0) return exitBeam1; //the last sphere does not have any sphere behind

            u2 = u1p;
            l2 = l1p - Interval.Dequeue();
            Beam incidentBeam2 = new Beam(l2, u2);

            return GaussianRefraction(incidentBeam2, Radius, RefractiveIndex, Interval);
        }


        private Beam RealRefraction(Beam incidentBeam1, Queue<double> Radius,
            Queue<double> RefractiveIndex, Queue<double> Interval) //Calculate exit beam using recursion
        {

            double u1p, l1p, u2, l2;
            double incidentAngle, exitAngle;
            double radius = Radius.Dequeue();
            double n = RefractiveIndex.Dequeue();
            double np = RefractiveIndex.Peek(); //Do not delete np item for later use: n2 = n1p

            double sinI;
            sinI = (incidentBeam1.l - radius) * Math.Sin(incidentBeam1.u) / radius;
            if (Math.Abs(sinI) > 1)
            {
                MessageBox.Show("入射光线超半球！");
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
                MessageBox.Show("发生全反射！");
            }
            exitAngle = Math.Asin(sinIp);

            IncidentAngleQ.Enqueue(incidentAngle);
            ExitAngleQ.Enqueue(exitAngle);

            u1p = incidentAngle + incidentBeam1.u - exitAngle;
            l1p = radius + radius * sinIp / Math.Sin(u1p);

            Beam exitBeam1 = new Beam(l1p, u1p);
            if (Radius.Count == 0) return exitBeam1; // the last sphere does not have any sphere behind

            u2 = u1p;
            l2 = l1p - Interval.Dequeue();
            Beam incidentBeam2 = new Beam(l2, u2);

            return RealRefraction(incidentBeam2, Radius, RefractiveIndex, Interval);
        }

        private Beam initOnaxialGaussian()
        {
            Beam beam;
            Queue<double> RefractiveIndex = new Queue<double>(RefractiveIndexQ);
            Queue<double> Radius = new Queue<double>(RadiusQ);
            Queue<double> Interval = new Queue<double>(IntervalQ);
            if (isInfinite == true)
            {
                RefractiveIndex = new Queue<double>(RefractiveIndexQ);
                Radius = new Queue<double>(RadiusQ);
                Interval = new Queue<double>(IntervalQ);
                double n_tmp = RefractiveIndex.Dequeue();
                double np_tmp = RefractiveIndex.Peek();
                double r_tmp = Radius.Dequeue();
                double fp = np_tmp / (np_tmp - n_tmp) * r_tmp;
                double lp_tmp;
                if (Interval.Count != 0)
                    lp_tmp = fp - Interval.Dequeue();
                else
                    lp_tmp = fp;
                beam = new Beam(lp_tmp, -pupilDiameter / 2 / fp);

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

        private Dictionary<string, Beam> initOnAxialReal(double[] K2s)
        {
            Dictionary<string, Beam> beam = new Dictionary<string, Beam> { };
            Queue<double> RefractiveIndex = new Queue<double>(RefractiveIndexQ);
            Queue<double> Radius = new Queue<double>(RadiusQ);
            Queue<double> Interval = new Queue<double>(IntervalQ);
            int flag = 0;
            foreach (double K2 in K2s)
            {
                RefractiveIndex = new Queue<double>(RefractiveIndexQ);
                Radius = new Queue<double>(RadiusQ);
                Interval = new Queue<double>(IntervalQ);
                if (isInfinite == true)
                {
                    flag = 1;
                    double n_tmp = RefractiveIndex.Dequeue();
                    double np_tmp = RefractiveIndex.Peek();
                    double r_tmp = Radius.Dequeue();
                    double fp = np_tmp / (np_tmp - n_tmp) * r_tmp;
                    double lp_tmp;
                    if (Interval.Count != 0)
                        lp_tmp = fp - Interval.Dequeue();
                    else
                        lp_tmp = fp;
                    beam.Add(K2.ToString(), new Beam(lp_tmp, Math.Sin(Math.Atan(K2 * pupilDiameter / 2 / fp))));
                }
                else
                    beam.Add(K2.ToString(), new Beam(obj.objDistance, K2 * Math.Sin(obj.apertureAngle)));
            }
            if (flag == 1)
            {
                IntervalQ.Dequeue();
                RadiusQ.Dequeue();
            }
            return beam;
        }

        private Dictionary<string, Beam> initOffAxialReal(double[] K1s, double[] K2s)
        {
            Dictionary<string, Beam> beam = new Dictionary<string, Beam> { };
            foreach (double K1 in K1s)
            {
                foreach (double K2 in K2s)
                {
                    if (isInfinite == true)
                        beam.Add(K1.ToString() + "  " + K2.ToString(), new Beam(K2 * pupilDiameter / 2 / Math.Tan(K1 * obj.fieldAngle), K1 * obj.fieldAngle));
                    else
                    {
                        double tanU1;
                        tanU1 = (K1 * obj.objHeight - K2 * pupilDiameter / 2) / obj.objDistance;
                        beam.Add(K1.ToString() + "  " + K2.ToString(), new Beam(K2 * pupilDiameter / 2 / tanU1, Math.Atan(tanU1)));
                    }
                }
            }

            return beam;
        }

        private void sphericalAber(Dictionary<string, Beam> outputReal, Beam outputGaussian)
        {
            Dictionary<string, double> SphericalAber = new Dictionary<string, double>();

            Beam outputR1 = outputReal["1"];
            Beam outputR2 = outputReal["0.7"];
            SphericalAber.Add("1", outputR1.l - outputGaussian.l);
            SphericalAber.Add("0.7", outputR2.l - outputGaussian.l);
            MessageBox.Show("全孔径球差：" + SphericalAber["1"].ToString() + "\n 0.7孔径球差:" + SphericalAber["0.7"]);

        }
    }
}
