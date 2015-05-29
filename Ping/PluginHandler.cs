using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Net.NetworkInformation;

namespace lucidcode.LucidScribe.Plugin.PingTime
{
  public class PluginHandler : lucidcode.LucidScribe.Interface.LucidPluginBase
  {

      static Thread pingTimer;
      static double pingTime;
      static Boolean disposed = false;

    public override string Name
    {
      get
      {
        return "Ping";
      }
    }

    public override bool Initialize()
    {
      try
      {
        pingTimer = new Thread(new ThreadStart(PingLoop));
        pingTimer.Start();
        return true;
      }
      catch (Exception ex)
      {
        throw (new Exception("The '" + Name + "' plugin failed to initialize: " + ex.Message));
      }
    }

    public override void Dispose()
    {
        disposed = true;
    }

    public void PingLoop()
    {
        do
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send("google.com");
                if (reply.Status == IPStatus.Success)
                {
                    pingTime = reply.RoundtripTime;
                }
            }
            catch (Exception ex)
            { }
            Application.DoEvents();
        }
        while (!disposed);
    }

    public override double Value
    {
      get
      {
          return pingTime;
      }
    }

  }
}

