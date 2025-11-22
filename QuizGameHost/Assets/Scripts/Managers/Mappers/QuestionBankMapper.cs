using Assets.SharedAssets.Networking.Data;
using System.Collections.Generic;
using System.Linq;

namespace Assets.SharedAssets.Networking.Mappers
{
    public static class QuestionBankMapper
    {
        public static QuestionBank ToModel(this QuestionBankDto dto)
        {
            return new QuestionBank(dto.Id, dto.Title);
        }

        public static List<QuestionBank> ToModelList(this List<QuestionBankDto> dtos)
        {
            return dtos.Select(ToModel).ToList();
        }
    }
}
