using System;

namespace BizHawk.Emulation.Common.Cores.MC6800
{
	public partial class MC6800
	{
		// this contains the vectors of instrcution operations
		// NOTE: This list is NOT confirmed accurate for each individual cycle

		private void NOP_()
		{
			cur_instr = new ushort[]
						{IDLE,
						OP };
		}

		private void INC_16(ushort src_l, ushort src_h)
		{
			cur_instr = new ushort[]
						{INC16,  src_l, src_h,
						IDLE,
						IDLE,
						OP };
		}


		private void DEC_16(ushort src_l, ushort src_h)
		{
			cur_instr = new ushort[]
						{DEC16, src_l, src_h,
						IDLE,
						IDLE,
						OP };
		}

		private void REG_OP(ushort operation, ushort dest, ushort src)
		{
			cur_instr = new ushort[]
						{operation, dest, src,
						IDLE,
						IDLE,
						OP };
		}

		private void SWI_()
		{
			cur_instr = new ushort[]
						{IDLE,
						DEC16, SPl, SPh,
						WR, SPl, SPh, B,
						DEC16, SPl, SPh,
						WR, SPl, SPh, A,
						DEC16, SPl, SPh,
						WR, SPl, SPh, Ixh,
						DEC16, SPl, SPh,
						WR, SPl, SPh, Ixl,
						DEC16, SPl, SPh,
						WR, SPl, SPh, PCh,
						DEC16, SPl, SPh,
						WR, SPl, SPh, PCl,
						ASGN, Z, 0xFA,
						ASGN, W, 0xFF,
						RD, PCl, Z, W,
						INC16, Z, W,
						RD, PCh, Z, W,
						OP };
		}

		private void WAI_()
		{
			cur_instr = new ushort[]
						{IDLE,
						DEC16, SPl, SPh,
						WR, SPl, SPh, B,
						DEC16, SPl, SPh,
						WR, SPl, SPh, A,
						DEC16, SPl, SPh,
						WR, SPl, SPh, Ixh,
						DEC16, SPl, SPh,
						WR, SPl, SPh, Ixl,
						DEC16, SPl, SPh,
						WR, SPl, SPh, PCh,
						DEC16, SPl, SPh,
						WR, SPl, SPh, PCl,
						WAI };
		}

		private void JR_COND(bool cond)
		{
			if (cond)
			{
				cur_instr = new ushort[]
							{IDLE,
							IDLE,
							IDLE,
							RD, W, PCl, PCh,
							IDLE,
							INC16, PCl, PCh,
							IDLE,
							ASGN, Z, 0,
							IDLE,
							ADDS, PCl, PCh, W, Z,
							IDLE,
							OP };
			}
			else
			{
				cur_instr = new ushort[]
							{IDLE,
							IDLE,
							IDLE,
							RD, Z, PCl, PCh,
							IDLE,
							INC16, PCl, PCh,
							IDLE,
							OP };
			}
		}

		private void JP_COND(bool cond)
		{
			if (cond)
			{
				cur_instr = new ushort[]
							{IDLE,
							IDLE,
							IDLE,
							RD, W, PCl, PCh,
							IDLE,
							INC16, PCl, PCh,
							IDLE,
							RD, Z, PCl, PCh,
							IDLE,
							INC16, PCl, PCh,
							IDLE,
							TR, PCl, W,
							IDLE,
							TR, PCh, Z,
							IDLE,
							OP };
			}
			else
			{
				cur_instr = new ushort[]
							{IDLE,
							IDLE,
							IDLE,
							RD, W, PCl, PCh,
							IDLE,
							INC16, PCl, PCh,
							IDLE,
							RD, Z, PCl, PCh,
							IDLE,
							INC16, PCl, PCh,
							IDLE,
							OP };
			}
		}

		private void RET_()
		{
			cur_instr = new ushort[]
						{IDLE,
						IDLE,
						IDLE,
						RD, PCl, SPl, SPh,
						IDLE,
						INC16, SPl, SPh,
						IDLE,
						RD, PCh, SPl, SPh,
						IDLE,
						INC16, SPl, SPh,
						IDLE,
						IDLE,
						IDLE,
						IDLE,
						IDLE,
						OP };
		}

		private void RETI_()
		{
			cur_instr = new ushort[]
						{IDLE,
						IDLE,
						IDLE,
						RD, PCl, SPl, SPh,
						IDLE,
						INC16, SPl, SPh,
						IDLE,
						RD, PCh, SPl, SPh,
						IDLE,
						INC16, SPl, SPh,
						IDLE,
						IDLE,
						IDLE,
						IDLE,
						OP };
		}

		private void BRS()
		{

			cur_instr = new ushort[]
						{IDLE,
						IDLE,
						IDLE,
						RD, ALU, PCl, PCh,
						INC16, PCl, PCh,
						IDLE,
						DEC16, SPl, SPh,
						WR, SPl, SPh, PCh,
						IDLE,
						IDLE,
						DEC16, SPl, SPh,
						WR, SPl, SPh, PCl,
						ASGN, W, 0,
						ADDS, PCl, PCh, ALU, W,
						OP };
		}

		private void INT_OP(ushort operation, ushort src)
		{
			cur_instr = new ushort[]
						{operation, src,
						OP };
		}

		private void BIT_OP(ushort operation, ushort bit, ushort src)
		{
			cur_instr = new ushort[]
						{operation, bit, src,
						IDLE,
						IDLE,
						OP };
		}

		private void PUSH_(ushort src)
		{
			cur_instr = new ushort[]
						{IDLE,
						IDLE,
						IDLE,
						IDLE,
						IDLE,
						DEC16, SPl, SPh,
						IDLE,
						WR, SPl, SPh, src,
						OP };
		}

		// NOTE: this is the only instruction that can write to P
		// but the top 2 bits of P are always 1, so instead of putting a special check for every read op
		// let's just put a special operation here specifically for P
		private void POP_(ushort src)
		{
			if (src != P)
			{
				cur_instr = new ushort[]
							{IDLE,
							IDLE,
							IDLE,
							RD, src, SPl, SPh,
							IDLE,
							INC16, SPl, SPh,
							IDLE,
							OP };
			}
			else
			{
				cur_instr = new ushort[]
							{IDLE,
							IDLE,
							IDLE,
							RD_P, src, SPl, SPh,
							IDLE,
							INC16, SPl, SPh,
							IDLE,
							OP };
			} 
		}

		private void JAM_()
		{
			cur_instr = new ushort[]
						{JAM,
						IDLE,
						IDLE,
						IDLE };
		}

		private void TR_16_(ushort dest_l, ushort dest_h, ushort src_l, ushort src_h)
		{
			cur_instr = new ushort[]
						{IDLE,
						TR_16, dest_l, dest_h, src_l, src_h,
						IDLE,
						OP };
		}

		private void CP_16_IMM(ushort src_l, ushort src_h)
		{
			cur_instr = new ushort[]
						{RD, Z, PCl, PCh,
						INC16, PCl, PCh,
						RD, W, PCl, PCh,
						INC16, PCl, PCh,
						CP16, Z, W, src_l, src_h,
						IDLE,
						OP };
		}

		private void REG_OP_IMM(ushort operation, ushort dest, ushort src_l, ushort src_h)
		{
			cur_instr = new ushort[]
						{IDLE,
						IDLE,
						IDLE,
						RD, Z, src_l, src_h,
						IDLE,
						operation, dest, Z,
						INC16, src_l, src_h,
						OP };
		}


		private void LD_IMM_16(ushort dest_l, ushort dest_h, ushort src_l, ushort src_h)
		{
			cur_instr = new ushort[]
						{IDLE,
						IDLE,
						IDLE,
						RD, dest_l, src_l, src_h,
						IDLE,
						INC16, src_l, src_h,
						IDLE,
						RD, dest_h, src_l, src_h,
						IDLE,
						INC16, src_l, src_h,
						IDLE,
						OP };
		}

		private void LD_16_IMM(ushort dest_l, ushort dest_h, ushort src_l, ushort src_h)
		{
			cur_instr = new ushort[]
						{IDLE,
						IDLE,
						IDLE,
						WR, dest_l, dest_h, src_l,
						IDLE,
						INC16, dest_l, src_h,
						IDLE,
						WR, dest_l, dest_h, src_h,
						IDLE,
						INC16, dest_l, dest_h,
						IDLE,
						OP };
		}
	}
}