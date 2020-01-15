using System;
using System.Collections.Generic;
using DAL;
using Model;

namespace BLL
{
    public class AttachmentsBll
    {
        public Attachments GetObjectById(Guid id)
        {
            var szServices = new DbHelperEfSql<Attachments>();
            return szServices.SearchBySingle(c => c.AttachmentID == id);
        }

        public bool PhyDelete(Guid id)
        {
            var szServices = new DbHelperEfSql<Attachments>();
            return szServices.PhysicalDeleteByCondition(c => c.AttachmentID == id);
        }
        public bool PhyDelete(Guid pid, int tableType)
        {
            var szServices = new DbHelperEfSql<Attachments>();
            return szServices.PhysicalDeleteByCondition(c => c.ParentGUID == pid && c.TableType == tableType);
        }

        public bool PhyDelete(int pid, int tableType)
        {
            var szServices = new DbHelperEfSql<Attachments>();
            return szServices.PhysicalDeleteByCondition(c => c.ParentID == pid && c.TableType == tableType);
        }
        public bool PhyDelete(Guid id, Guid pid, int tableType)
        {
            var szServices = new DbHelperEfSql<Attachments>();
            return szServices.PhysicalDeleteByCondition(c => c.AttachmentID == id && c.ParentGUID == pid && c.TableType == tableType);
        }

        public bool PhyDelete(Guid id, int pid, int tableType)
        {
            var szServices = new DbHelperEfSql<Attachments>();
            return szServices.PhysicalDeleteByCondition(c => c.AttachmentID == id && c.ParentID == pid && c.TableType == tableType);
        }

        public int Count(int pid, int tableType)
        {
            var szServices = new DbHelperEfSql<Attachments>();
            return szServices.Count(c => c.ParentID == pid && c.TableType == tableType);
        }

        public int Count(Guid pid, int tableType)
        {
            var szServices = new DbHelperEfSql<Attachments>();
            return szServices.Count(c => c.ParentGUID == pid && c.TableType == tableType);
        }

        public IList<Attachments> SearchListByPid(int pid, int tableType)
        {
            var szServices = new DbHelperEfSql<Attachments>();
            return szServices.SearchListByCondition(c => c.ParentID == pid && c.TableType == tableType);
        }
        public IList<Attachments> SearchListByPid(Guid pid, int tableType)
        {
            var szServices = new DbHelperEfSql<Attachments>();
            return szServices.SearchListByCondition(c => c.ParentGUID == pid && c.TableType == tableType);
        }
        public bool AddAll(IList<Attachments> list)
        {
            var szServices = new DbHelperEfSql<Attachments>();
            return szServices.AddAll(list);
        }

        public bool Add(Attachments item)
        {
            var szServices = new DbHelperEfSql<Attachments>();
            return szServices.Add(item);
        }
        public bool AddPhotoSrc(IList<Attachments> list)
        {
            var szServices = new DbHelperEfSql<Attachments>();
            return szServices.AddL(list);
        }
    }
}
