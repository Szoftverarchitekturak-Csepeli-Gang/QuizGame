using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IPresenter<TScreen>
    where TScreen : ScreenController
{
    void AttachScreen(TScreen screen);
    void DetachScreen();
}