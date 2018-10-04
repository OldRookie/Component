using AutoMapper;
using Component.Data.Repository;
using Component.Infrastructure;
using Component.Infrastructure.Utility;
using Component.Model.Domain;
using Component.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Application
{
    public class BaseFormApp
    {
        private static void MergeFormDetail(BaseForm drawingMgtWFBaseForm, List<FormDetail> drawingMgtWFFormDetail)
        {
            BaseRepository<FormDetail> _drawingMgtWFFormDetailBaseRepository = new BaseRepository<FormDetail>();

            //表单详情更新
            for (int i = 0; i < drawingMgtWFFormDetail.Count; i++)
            {
                var currentItem = drawingMgtWFFormDetail[i];

                var originalItem = drawingMgtWFBaseForm.FormDetail.FirstOrDefault(x => x.Id == currentItem.Id);
                if (originalItem != null)
                {
                    currentItem.MapTo<FormDetail>(originalItem, cfg =>
                    {
                        cfg.CreateMap<FormDetail, FormDetail>(MemberList.None)
                            .ForMember(x => x.Id, m => m.Ignore());
                    }, true);
                }
                else
                {
                    //currentItem.Id = Guid.NewGuid();
                    _drawingMgtWFFormDetailBaseRepository.Add(currentItem);
                }
            }
            drawingMgtWFBaseForm.FormDetail.Where(x => !drawingMgtWFFormDetail.Any(y => y.Id == x.Id)).ToList().ForEach(x =>
            {
                _drawingMgtWFFormDetailBaseRepository.Delete(x);
            });
        }
    }
}
