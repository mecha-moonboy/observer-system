using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Orbit
{

    private decimal _time_set;
    private decimal _eccentricity;
    private decimal _semi_major_axis;
    private decimal _inclination;
    private decimal _longitude_ascending_node;
    private decimal _argumaent_periapsis;
    private decimal _mean_anomaly;
    private decimal _u;


    public Vector3Decimal OrbitPositionAtTime(decimal T)
    {

        return Vector3Decimal.zero;

    }

    /*
    PosVel Lambert(Vector3Decimal initial_position, Vector3Decimal terminal_position, decimal flight_time, decimal gravitational_parameter, bool counter_clockwise = true)
    {
        PosVel ret;


        

        return ret;

        decimal _func(decimal z, decimal targett, decimal r1pr2, decimal a, decimal mu )
        {
            decimal val_y = r1pr2 - a * (1 - z * _Sz(z)) / DecimalMath.DecimalEx.Sqrt(_Cz(z));
            decimal val_x = DecimalMath.DecimalEx.Sqrt(val_y / _Cz(z));
            decimal t = (DecimalMath.DecimalEx.Pow(val_x, 3) * _Sz(z) + a * DecimalMath.DecimalEx.Sqrt(val_y)) / DecimalMath.DecimalEx.Sqrt(gravitational_parameter);

            return t - targett;
        }
        decimal _Cz(decimal z)
        {
            if(z < 0)
            {
                return (1 - Cosh(DecimalMath.DecimalEx.Sqrt(-1 * z))) / z;
            }
            else
            {
                return (1 - Cosh(DecimalMath.DecimalEx.Sqrt(z))) / z;
            }
        }
        decimal _Sz(decimal z)
        {
            if (z < 0)
            {
                decimal sqz = DecimalMath.DecimalEx.Sqrt(-1 * z);
                return (Sinh(sqz) - sqz) / DecimalMath.DecimalEx.Pow(sqz, 3);
            }
            else
            {
                decimal sqz = DecimalMath.DecimalEx.Sqrt(z);
                return (sqz - Sinh(sqz)) / DecimalMath.DecimalEx.Pow(sqz, 3);
            }
        }
        decimal Cosh(decimal x)
        {
            return (DecimalMath.DecimalEx.Pow(DecimalMath.DecimalEx.E, x) + DecimalMath.DecimalEx.Pow(DecimalMath.DecimalEx.E, -x)) / 2;
        }
        decimal Sinh(decimal x)
        {
            return (DecimalMath.DecimalEx.Pow(DecimalMath.DecimalEx.E, x) - DecimalMath.DecimalEx.Pow(DecimalMath.DecimalEx.E, -x)) / 2;
        }

        decimal r1 = DecimalMath.DecimalEx.Sqrt(Vector3Decimal.Dot(initial_position, terminal_position));
        decimal r2 = DecimalMath.DecimalEx.Sqrt(Vector3Decimal.Dot( terminal_position , terminal_position));

        Vector3Decimal r1cr2 = Vector3Decimal.Cross(initial_position, terminal_position);
        decimal r1dr2 = Vector3Decimal.Dot(initial_position, terminal_position);

        decimal sindnu = DecimalMath.DecimalEx.Sqrt(Vector3Decimal.Dot(r1cr2, r1cr2)) / r1 / r2;


        if (r1cr2.y < 0) sindnu = -sindnu;

        if (!counter_clockwise) sindnu = -sindnu;

        decimal cosdnu = r1dr2 / r1 / r2;
        decimal a = DecimalMath.DecimalEx.Sqrt(r1 * r2) * sindnu / DecimalMath.DecimalEx.Sqrt(1 - cosdnu);
        decimal r1pr2 = r1 + r2;

        decimal dnu = DecimalMath.DecimalEx.ATan2(sindnu, cosdnu);
        if (dnu < 0) dnu += (decimal)(Mathf.PI * 2);

        if(dnu < (decimal)0.001 || dnu > (decimal)(Mathf.PI * 2 - 0.001))
        {
            Debug.LogWarning("Difference in true anomaly is too small.");
        }
        if(DecimalMath.DecimalEx.Pow((dnu - (decimal)Mathf.PI),2) < (decimal)0.00001)
        {
            Debug.LogWarning("Two points are placed opposite each other");
        }

        decimal inf = decimal.MaxValue;

        decimal minb1 = (decimal)((-1) * Mathf.Pow((Mathf.PI * 2), 2));

        bool found = false;
        for(int i = 0; i < 10; i++)
        {
            decimal b2 = (decimal)(Mathf.Pow((Mathf.PI * 2), 2) - 1 / Mathf.Pow(10, i));
            decimal test = _func(b2, flight_time, r1pr2, a, gravitational_parameter);
            if(test > 0)
            {
                found = true;
                break;
            }
            if (!found)
            {
                Debug.LogWarning("Could not solve lamberts problem.");
            }
            
        }

        


    }
    */

    struct PosVel
    {
        Vector3Decimal pos;
        Vector3Decimal vel;

        public PosVel(Vector3Decimal pos, Vector3Decimal vel)
        {
            this.pos = pos;
            this.vel = vel;
        }
        public Vector3Decimal Pos()
        {
            return pos;
        }
        public Vector3Decimal Vel()
        {
            return vel;
        }

    }

}

