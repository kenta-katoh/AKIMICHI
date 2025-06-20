using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi
{
    public class EventAnimeController : AnimeController
    {
        protected override void OnFinished()
        {
            base.OnFinished();
            this.animator.SetBool("IsEvent", false);
            this.animator.Play("InitEvent");
        }
    }
}
