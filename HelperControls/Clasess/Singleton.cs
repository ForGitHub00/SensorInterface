﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperControls {
   public  class Singleton {
        private static readonly Singleton instance = new Singleton();

        public string Name { get; set; }

        private Singleton() {
            Name = Guid.NewGuid().ToString();
            _MAP = new List<RPoint>();
            Position = new RPoint(x: Double.MinValue);
        }
        public List<RPoint> _MAP;
        public RPoint Position;
        
        public static Singleton GetInstance() {
            return instance;
        }
    }
}
