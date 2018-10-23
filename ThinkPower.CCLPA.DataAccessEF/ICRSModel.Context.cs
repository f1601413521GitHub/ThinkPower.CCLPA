﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ThinkPower.CCLPA.DataAccessEF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ICRSEntities : DbContext
    {
        public ICRSEntities()
            : base("name=ICRSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<OD_VIP> OD_VIP { get; set; }
        public virtual DbSet<RG_ID> RG_ID { get; set; }
    
        public virtual int SP_ICRS_CONSUME_QUERY(string lS_CARD_ACCT_ID, string lS_CARD_ACCT_ID_SEQ, ObjectParameter lL_TOT_AMT_CONSUME, ObjectParameter lL_REMAIN, ObjectParameter lS_RISK_LEVEL, ObjectParameter lS_SPEC_FLAG, ObjectParameter lS_RESP_CODE)
        {
            var lS_CARD_ACCT_IDParameter = lS_CARD_ACCT_ID != null ?
                new ObjectParameter("LS_CARD_ACCT_ID", lS_CARD_ACCT_ID) :
                new ObjectParameter("LS_CARD_ACCT_ID", typeof(string));
    
            var lS_CARD_ACCT_ID_SEQParameter = lS_CARD_ACCT_ID_SEQ != null ?
                new ObjectParameter("LS_CARD_ACCT_ID_SEQ", lS_CARD_ACCT_ID_SEQ) :
                new ObjectParameter("LS_CARD_ACCT_ID_SEQ", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_ICRS_CONSUME_QUERY", lS_CARD_ACCT_IDParameter, lS_CARD_ACCT_ID_SEQParameter, lL_TOT_AMT_CONSUME, lL_REMAIN, lS_RISK_LEVEL, lS_SPEC_FLAG, lS_RESP_CODE);
        }
    
        public virtual int SP_ICRS_TO_CCAS_ADJ_NONTAX(string acard_acct_id, string acard_acct_id_seq, string aadj_area, Nullable<decimal> aadj_amt, string aadj_reason, string aadj_reason1, string aadj_reason2, string aadj_remark, string aadj_eff_start_date, string aadj_eff_end_date, string aadj_user, string aadj_user2, string aadj_country, string aadj_proj_code, Nullable<decimal> aadj_amt_2, ObjectParameter aresp_code)
        {
            var acard_acct_idParameter = acard_acct_id != null ?
                new ObjectParameter("Acard_acct_id", acard_acct_id) :
                new ObjectParameter("Acard_acct_id", typeof(string));
    
            var acard_acct_id_seqParameter = acard_acct_id_seq != null ?
                new ObjectParameter("Acard_acct_id_seq", acard_acct_id_seq) :
                new ObjectParameter("Acard_acct_id_seq", typeof(string));
    
            var aadj_areaParameter = aadj_area != null ?
                new ObjectParameter("Aadj_area", aadj_area) :
                new ObjectParameter("Aadj_area", typeof(string));
    
            var aadj_amtParameter = aadj_amt.HasValue ?
                new ObjectParameter("Aadj_amt", aadj_amt) :
                new ObjectParameter("Aadj_amt", typeof(decimal));
    
            var aadj_reasonParameter = aadj_reason != null ?
                new ObjectParameter("Aadj_reason", aadj_reason) :
                new ObjectParameter("Aadj_reason", typeof(string));
    
            var aadj_reason1Parameter = aadj_reason1 != null ?
                new ObjectParameter("Aadj_reason1", aadj_reason1) :
                new ObjectParameter("Aadj_reason1", typeof(string));
    
            var aadj_reason2Parameter = aadj_reason2 != null ?
                new ObjectParameter("Aadj_reason2", aadj_reason2) :
                new ObjectParameter("Aadj_reason2", typeof(string));
    
            var aadj_remarkParameter = aadj_remark != null ?
                new ObjectParameter("Aadj_remark", aadj_remark) :
                new ObjectParameter("Aadj_remark", typeof(string));
    
            var aadj_eff_start_dateParameter = aadj_eff_start_date != null ?
                new ObjectParameter("Aadj_eff_start_date", aadj_eff_start_date) :
                new ObjectParameter("Aadj_eff_start_date", typeof(string));
    
            var aadj_eff_end_dateParameter = aadj_eff_end_date != null ?
                new ObjectParameter("Aadj_eff_end_date", aadj_eff_end_date) :
                new ObjectParameter("Aadj_eff_end_date", typeof(string));
    
            var aadj_userParameter = aadj_user != null ?
                new ObjectParameter("Aadj_user", aadj_user) :
                new ObjectParameter("Aadj_user", typeof(string));
    
            var aadj_user2Parameter = aadj_user2 != null ?
                new ObjectParameter("Aadj_user2", aadj_user2) :
                new ObjectParameter("Aadj_user2", typeof(string));
    
            var aadj_countryParameter = aadj_country != null ?
                new ObjectParameter("Aadj_country", aadj_country) :
                new ObjectParameter("Aadj_country", typeof(string));
    
            var aadj_proj_codeParameter = aadj_proj_code != null ?
                new ObjectParameter("Aadj_proj_code", aadj_proj_code) :
                new ObjectParameter("Aadj_proj_code", typeof(string));
    
            var aadj_amt_2Parameter = aadj_amt_2.HasValue ?
                new ObjectParameter("Aadj_amt_2", aadj_amt_2) :
                new ObjectParameter("Aadj_amt_2", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_ICRS_TO_CCAS_ADJ_NONTAX", acard_acct_idParameter, acard_acct_id_seqParameter, aadj_areaParameter, aadj_amtParameter, aadj_reasonParameter, aadj_reason1Parameter, aadj_reason2Parameter, aadj_remarkParameter, aadj_eff_start_dateParameter, aadj_eff_end_dateParameter, aadj_userParameter, aadj_user2Parameter, aadj_countryParameter, aadj_proj_codeParameter, aadj_amt_2Parameter, aresp_code);
        }
    
        public virtual int SP_ICRS_TO_CCAS_ADJ_TAX(string aadj_action, string acard_acct_id, string acard_acct_id_seq, string aadj_proj_code, Nullable<decimal> aadj_amt, string aadj_effend_date, string aadj_user, ObjectParameter aresp_code)
        {
            var aadj_actionParameter = aadj_action != null ?
                new ObjectParameter("Aadj_action", aadj_action) :
                new ObjectParameter("Aadj_action", typeof(string));
    
            var acard_acct_idParameter = acard_acct_id != null ?
                new ObjectParameter("Acard_acct_id", acard_acct_id) :
                new ObjectParameter("Acard_acct_id", typeof(string));
    
            var acard_acct_id_seqParameter = acard_acct_id_seq != null ?
                new ObjectParameter("Acard_acct_id_seq", acard_acct_id_seq) :
                new ObjectParameter("Acard_acct_id_seq", typeof(string));
    
            var aadj_proj_codeParameter = aadj_proj_code != null ?
                new ObjectParameter("Aadj_proj_code", aadj_proj_code) :
                new ObjectParameter("Aadj_proj_code", typeof(string));
    
            var aadj_amtParameter = aadj_amt.HasValue ?
                new ObjectParameter("Aadj_amt", aadj_amt) :
                new ObjectParameter("Aadj_amt", typeof(decimal));
    
            var aadj_effend_dateParameter = aadj_effend_date != null ?
                new ObjectParameter("Aadj_effend_date", aadj_effend_date) :
                new ObjectParameter("Aadj_effend_date", typeof(string));
    
            var aadj_userParameter = aadj_user != null ?
                new ObjectParameter("Aadj_user", aadj_user) :
                new ObjectParameter("Aadj_user", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_ICRS_TO_CCAS_ADJ_TAX", aadj_actionParameter, acard_acct_idParameter, acard_acct_id_seqParameter, aadj_proj_codeParameter, aadj_amtParameter, aadj_effend_dateParameter, aadj_userParameter, aresp_code);
        }
    }
}
