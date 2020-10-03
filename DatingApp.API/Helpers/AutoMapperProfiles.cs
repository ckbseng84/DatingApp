using System;
using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        // Automapper use profile to understand the source and destination
        // of what is mapping
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl,
                                opt => opt.MapFrom(
                                    src => src.Photos.FirstOrDefault(
                                        p => p.IsMain).Url
                                        ))
                .ForMember(dest => dest.Age,
                            opt => opt.MapFrom(
                                src => src.DateOfBirth.CalculateAge()
                            ));
            CreateMap<User, UserForDetailDto>()
                .ForMember(dest => dest.PhotoUrl,
                                opt => opt.MapFrom(
                                    src => src.Photos.FirstOrDefault(
                                        p => p.IsMain).Url
                                        ))
                .ForMember(dest => dest.Age,
                            opt => opt.MapFrom(
                                src => src.DateOfBirth.CalculateAge()
                            )); ;
            CreateMap<Photo, PhotoForDetailDto>();
            CreateMap<UserForUpdateDto,User>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<MessageforCreationDto, Message>().ReverseMap();
            CreateMap<Message, MessageToReturnDto>()
                .ForMember(dest => dest.SenderPhotoUrl,
                                opt => opt.MapFrom(
                                    src => src.Sender.Photos.FirstOrDefault(
                                        p => p.IsMain).Url
                                )
                            )
                .ForMember(dest => dest.RecipientPhotoUrl,
                                opt => opt.MapFrom(
                                    src => src.Recipient.Photos.FirstOrDefault(
                                        p => p.IsMain).Url
                                )
                            );
        }
    }
}