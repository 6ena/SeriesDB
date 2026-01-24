using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SeriesDB.Series
{
    public interface ISerieAppService : ICrudAppService< //Define CRUD methods. Create, Read, Update, Delete.
        SerieDto, //used to show Serie
        int, //primary key of the Serie entity
        PagedAndSortedResultRequestDto, //used for paging/sorting
        CreateUpdateSerieDto,
        CreateUpdateSerieDto> //used to create/update a Serie
    {
    }
}
