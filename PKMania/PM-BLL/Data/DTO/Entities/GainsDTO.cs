﻿using PM_DAL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_BLL.Data.DTO.Entities
{
    public class GainsDTO
    {
        public int GainsSharingNr { get; set; }
        public int StartPlace { get; set; }
        public int EndPlace { get; set; }
        public int NumberChips { get; set; }
        public Decimal Percentage { get; set; }

        public GainsDTO(Gains g)
        {
            GainsSharingNr = g.GainsSharingNr;
            StartPlace = g.StartPlace;
            EndPlace = g.EndPlace;
            NumberChips = g.NumberChips;
            Percentage = g.Percentage;
        }
    }
}
