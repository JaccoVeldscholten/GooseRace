using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace Controller {
    public static class Data {
        public static Competition comp { get; set; }

        public static void Initialize() {
            comp = new Competition();
        }

        static void AddParticipants() {

           Goose goose1 = new Goose();
           comp.Participants.Add(new Goose());

        }
    }
}
