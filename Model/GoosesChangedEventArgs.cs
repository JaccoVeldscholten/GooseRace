using System;
namespace Model {
    public class GoosesChangedEventArgs : EventArgs {
        public Track Track { get; set; }

        public GoosesChangedEventArgs(Track track) {
            this.Track = track;
        }
    }
}
