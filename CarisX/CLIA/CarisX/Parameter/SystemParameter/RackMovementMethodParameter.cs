using Oelco.CarisX.Const;

namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// ���b�N�ړ�����
    /// </summary>
	public class RackMovementMethodParameter : AttachmentParameter
	{
        #region [�v���p�e�B]

        /// <summary>
        /// ���b�N�ړ������̐ݒ�^�擾
        /// </summary>
        public RackMovementMethodKind RackMovementMethod { get; set; } = RackMovementMethodKind.Performance;

        #endregion
	}
	 
}
 
