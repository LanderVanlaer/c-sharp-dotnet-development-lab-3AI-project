using AutoMapper;

namespace c_sharp_dotnet_development_lab_3AI_project.database.entities.dto.payment_record;

public class ProfilePaymentRecord : Profile
{
    public ProfilePaymentRecord()
    {
        CreateMap<PaymentRecord, PaymentRecordReadDto>();
    }
}