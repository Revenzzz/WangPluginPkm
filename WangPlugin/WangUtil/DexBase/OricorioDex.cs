﻿using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WangPlugin.WangUtil.DexBase
{
    internal class OricorioDex
    {
        public static List<PKM> OricorioSets(ISaveFileProvider SAV, IPKMView Editor)
        {
            List<PKM> PKL = new();
            PKM pk;
            switch (SAV.SAV.Version)
            {
                case GameVersion.SN or GameVersion.MN or GameVersion.US
                or GameVersion.UM or GameVersion.SM or GameVersion.USUM:
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            pk = SearchDatabase.SearchPKM(SAV, Editor, 741, 30, j, false);
                            pk.Form = (byte)j;
                            pk.ClearNickname();
                            PKL.Add(pk);
                        }
                    }
                    break;
            }
            return PKL;
        }
    }
}
