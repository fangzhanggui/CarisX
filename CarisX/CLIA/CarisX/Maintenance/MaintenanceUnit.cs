using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.Comm;
using Oelco.Common.Utility;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;


namespace Oelco.CarisX.Maintenance
{
    interface IMaintenanceUnitStart
    {
        void Start(object StartComm, ModuleKind useModuleKind = ModuleKind.Slave);
    }
    interface IMaintenanceUnitPause
    {
        void Pause(ModuleKind useModuleKind = ModuleKind.Slave);
    }
    interface IMaintenanceUnitRestart
    {
        void Restart(ModuleKind useModuleKind = ModuleKind.Slave);
    }
    interface IMaintenanceUnitAbort
    {
        void Abort(ModuleKind useModuleKind = ModuleKind.Slave);
    }

    /// <summary> 
    /// ユニットテストコマンドのスタート送信部です。
    /// </summary>
    class UnitStart : IMaintenanceUnitStart
    {
        //ユニットテストコマンド
        public void Start(object StartComm, ModuleKind useModuleKind = ModuleKind.Slave)
        {
            if (ModuleKind.RackTransfer == useModuleKind)
            {
                Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer((RackTransferCommCommand_0039)StartComm);
            }
            else
            {
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((SlaveCommCommand_0439)StartComm);
            }

        }
    }
    /// <summary> 
    /// ユニット調整開始コマンドのスタート送信部です。
    /// </summary>
    class UnitStarAdjust : IMaintenanceUnitStart
    {
        //調整開始コマンド
        public void Start(object StartComm, ModuleKind useModuleKind = ModuleKind.Slave)
        {
            if (ModuleKind.RackTransfer == useModuleKind)
            {
                Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer((RackTransferCommCommand_0080)StartComm);
            }
            else
            {
                if( StartComm is SlaveCommCommand_0480)
                {
                    Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((SlaveCommCommand_0480)StartComm);

                }
                else if( StartComm is SlaveCommCommand_0497 )
                {
                    Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((SlaveCommCommand_0497)StartComm);
                }
            }

        }
    }
    /// <summary> 
    /// ユニット調整開始コマンドの終了送信部です。
    /// </summary>
    class UnitStarAdjustAbort : IMaintenanceUnitStart
    {
        //調整開始コマンド
        public void Start(object StartComm, ModuleKind useModuleKind = ModuleKind.Slave)
        {
            CommCommand cmd0X81;
            CommandKind cmd1X81;

            cmd0X81 = StartComm as CommCommand;

            if (ModuleKind.RackTransfer == useModuleKind)
            {
                cmd1X81 = CommandKind.RackTransferCommand1081;
            }
            else
            {
                cmd1X81 = CommandKind.Command1481;
            }

            Singleton<CarisXSequenceHelperManager>.Instance.Maintenance.MaintenanceAbortSequence(useModuleKind, cmd0X81, cmd1X81);
        }
    }

    /// <summary> 
    /// ユニット調整コマンドのスタート送信部です。
    /// </summary>
    class UnitAdjust : IMaintenanceUnitStart
    {
        //調整コマンド
        public void Start(object StartComm, ModuleKind useModuleKind = ModuleKind.Slave)
        {
            if (ModuleKind.RackTransfer == useModuleKind)
            {
                Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer((RackTransferCommCommand_0073)StartComm);
            }
            else
            {
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((SlaveCommCommand_0473)StartComm);
            }
        }
    }

    /// <summary> 
    /// 温度取得コマンドのスタート送信部です。
    /// </summary>  
    class UnitTempStart : IMaintenanceUnitStart
    {
        public void Start(object StartComm, ModuleKind useModuleKind = ModuleKind.Slave)
        {
            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((SlaveCommCommand_0437)StartComm);
        }
    }

    /// <summary> 
    /// センサーステータスコマンドのスタート送信部です。
    /// </summary>  
    class UnitSensorStart : IMaintenanceUnitStart
    {
        public void Start(object StartComm, ModuleKind useModuleKind = ModuleKind.Slave)
        {
            if (ModuleKind.RackTransfer == useModuleKind)
            {
                Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer((RackTransferCommCommand_0040)StartComm);
            }
            else
            {
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((SlaveCommCommand_0440)StartComm);
            }
        }
    }

    /// <summary> 
    /// ユニットテストコマンドのポーズ送信部です。
    /// </summary>
    class UnitPause : IMaintenanceUnitPause
    {
        public void Pause(ModuleKind useModuleKind = ModuleKind.Slave)
        {
            if (ModuleKind.RackTransfer == useModuleKind)
            {
                RackTransferCommCommand_0012 cmd = new RackTransferCommCommand_0012();
                cmd.Control = CommandControlParameter.Pause;
                cmd.Stop = RackTransferCommCommand_0012.StopParameter.AllStop;
                Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd);
            }
            else
            {
                SlaveCommCommand_0412 cmd = new SlaveCommCommand_0412();
                cmd.Control = CommandControlParameter.Pause;
                cmd.Stop = SlaveCommCommand_0412.StopParameter.AllStop;
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd);
            }
        }
    }

    /// <summary> 
    /// ユニットテストコマンドのリスタート送信部です。
    /// </summary>
    class UnitRestart : IMaintenanceUnitRestart
    {
        public void Restart(ModuleKind useModuleKind = ModuleKind.Slave)
        {
            if (ModuleKind.RackTransfer == useModuleKind)
            {
                RackTransferCommCommand_0012 cmd = new RackTransferCommCommand_0012();
                cmd.Control = CommandControlParameter.Restart;
                cmd.Stop = RackTransferCommCommand_0012.StopParameter.AllStop;
                Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd);
            }
            else
            {
                SlaveCommCommand_0412 cmd = new SlaveCommCommand_0412();
                cmd.Control = CommandControlParameter.Restart;
                cmd.Stop = SlaveCommCommand_0412.StopParameter.AllStop;
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd);
            }
        }
    }

    /// <summary> 
    /// ユニットテストコマンドのAbort送信部です。
    /// </summary>
    class UnitStop : IMaintenanceUnitAbort
    {
        public void Abort(ModuleKind useModuleKind = ModuleKind.Slave)
        {
            CommCommand cmd0X12;
            CommandKind cmd1X12;

            if (ModuleKind.RackTransfer == useModuleKind)
            {
                RackTransferCommCommand_0012 cmd = new RackTransferCommCommand_0012();
                cmd.Control = CommandControlParameter.Abort;
                cmd.Stop = RackTransferCommCommand_0012.StopParameter.AllStop;
                //Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd);
                cmd0X12 = cmd;
                cmd1X12 = CommandKind.RackTransferCommand1012;
            }
            else
            {
                SlaveCommCommand_0412 cmd = new SlaveCommCommand_0412();
                cmd.Control = CommandControlParameter.Abort;
                cmd.Stop = SlaveCommCommand_0412.StopParameter.AllStop;
                //Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd);
                cmd0X12 = cmd;
                cmd1X12 = CommandKind.Command1412;
            }

            Singleton<CarisXSequenceHelperManager>.Instance.Maintenance.MaintenanceAbortSequence(useModuleKind, cmd0X12, cmd1X12);
        }
    }

}
