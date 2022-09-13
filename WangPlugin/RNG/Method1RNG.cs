using PKHeX.Core;

namespace WangPlugin
{
    internal static class Method1RNG
    {
        private const int shift = 16;

        public static uint Next(uint seed) => LCRNG.Next(seed);

        public static bool GenPkm(ref PKM pk,uint seed, bool[] shiny,bool[] IV)
        {
            var pidLower = LCRNG.Next(seed) >> shift;
            var pidUpper = LCRNG.Advance(seed, 2) >> shift;
            var dvLower = LCRNG.Advance(seed, 3) >> shift;
            var dvUpper = LCRNG.Advance(seed, 4) >> shift;
            var pid = CombineRNG(pidUpper, pidLower, shift);
            var pidR = CombineRNG(pidLower, pidUpper, shift);
            var ivs = DvsToIVs(dvUpper, dvLower);
            var FlawlessIVs = 0;
            uint MAX = 31;
            if (pk.Species == 201)
            {
                pk.PID = pidR;
            }
            else
            {
                pk.PID = pid;
            }
            if (!CheckShiny(pk.PID, pk.TID, pk.SID,  shiny) )
            {
                return false;
            }
            if(IV[0]&&ivs[1]!=0)
            {
                return false;
            }
            if(IV[1]&&ivs[5]!=0)
            {
                return false;
            }
            pk.IV_HP = (int)ivs[0];
            pk.IV_ATK = (int)ivs[1];
            pk.IV_DEF = (int)ivs[2];
            pk.IV_SPA = (int)ivs[3];
            pk.IV_SPD = (int)ivs[4];
            pk.IV_SPE = (int)ivs[5];
            pk.Nature =(int)(pid %100% 25);
            pk.Gender = GenderApplicator.GetSaneGender(pk);
            pk.RefreshAbility((int)(pk.PID & 1));
            if (IV[2] == true)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (ivs[i] == MAX)
                        FlawlessIVs++;
                }
                if (FlawlessIVs is 3 or > 3)
                {
                    return true;
                }
                    return false;
            }
            var Info = new LegalityAnalysis(pk);
            if (Info.Info.FrameMatches == false)
                return false;
            return true;
        }
        private static uint[] DvsToIVs(uint dvUpper, uint dvLower)
        {
            return new uint[]
            {
                dvLower & 0x1f,
                (dvLower & 0x3E0) >> 5,
                (dvLower & 0x7C00) >> 10,
                (dvUpper & 0x3E0) >> 5,
                (dvUpper & 0x7C00) >> 10,
                dvUpper & 0x1f,
            };
        }
        private static bool CheckShiny(uint pid, int TID, int SID, bool[] shiny)
        {
            var s = (uint)(TID ^ SID) ^ ((pid >> 16) ^ (pid & 0xFFFF));
            if (shiny[0])
                return true;
            else if (shiny[1] && s < 8)
                return true;
            else if (shiny[2] && s < 8 && s != 0)
                return true;
            else if (shiny[3] && s == 0 )
                return true;
            else if (shiny[4] && s == 1)
                return true;
            else
                return false;
        }

        private static uint CombineRNG(uint upper, uint lower, uint shift)
        {
            return (upper << (int)shift) + lower;
        }
    }
}