﻿using System;
using System.Threading.Tasks;
using CompanyName.MyMeetings.API.Configuration.Authorization;
using CompanyName.MyMeetings.Modules.Meetings.Application.Contracts;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.AddMeetingAttendee;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.AddMeetingNotAttendee;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.CancelMeeting;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.ChangeMeetingMainAttributes;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.ChangeNotAttendeeDecision;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.CreateMeeting;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.RemoveMeetingAttendee;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.SetMeetingAttendeeRole;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.SetMeetingHostRole;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.SignOffMemberFromWaitlist;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.SignUpMemberToWaitlist;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.API.Modules.Meetings.Meetings
{
    [Route("api/meetings/meetings")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly IMeetingsModule _meetingsModule;

        public MeetingsController(IMeetingsModule meetingsModule)
        {
            _meetingsModule = meetingsModule;
        }

        [HttpPost("")]
        [HasPermission(MeetingsPermissions.CreateNewMeeting)]
        public async Task<IActionResult> CreateNewMeeting([FromBody] CreateMeetingRequest request)
        {
            await _meetingsModule.ExecuteCommandAsync(new CreateMeetingCommand(
                request.MeetingGroupId,
                request.Title,
                request.TermStartDate,
                request.TermEndDate,
                request.Description,
                request.MeetingLocationName,
                request.MeetingLocationAddress,
                request.MeetingLocationPostalCode,
                request.MeetingLocationCity,
                request.AttendeesLimit,
                request.GuestsLimit,
                request.RSVPTermStartDate,
                request.RSVPTermEndDate,
                request.EventFeeValue,
                request.EventFeeCurrency,
                request.HostMemberIds));

            return Ok();
        }

        [HttpPut("{meetingId}")]
        [HasPermission(MeetingsPermissions.EditMeeting)]
        public async Task<IActionResult> EditMeeting(
            [FromRoute] Guid meetingId,
            [FromBody] ChangeMeetingMainAttributesRequest mainAttributesRequest)
        {
            await _meetingsModule.ExecuteCommandAsync(new ChangeMeetingMainAttributesCommand(
                meetingId,
                mainAttributesRequest.Title,
                mainAttributesRequest.TermStartDate,
                mainAttributesRequest.TermEndDate,
                mainAttributesRequest.Description,
                mainAttributesRequest.MeetingLocationName,
                mainAttributesRequest.MeetingLocationAddress,
                mainAttributesRequest.MeetingLocationPostalCode,
                mainAttributesRequest.MeetingLocationCity,
                mainAttributesRequest.AttendeesLimit,
                mainAttributesRequest.GuestsLimit,
                mainAttributesRequest.RSVPTermStartDate,
                mainAttributesRequest.RSVPTermEndDate,
                mainAttributesRequest.EventFeeValue,
                mainAttributesRequest.EventFeeCurrency));

            return Ok();
        }

        [HttpPost("{meetingId}/attendees")]
        [HasPermission(MeetingsPermissions.AddMeetingAttendee)]
        public async Task<IActionResult> AddMeetingAttendee(
            [FromRoute] Guid meetingId,
            [FromBody] AddMeetingAttendeeRequest attendeeRequest)
        {
            await _meetingsModule.ExecuteCommandAsync(new AddMeetingAttendeeCommand(
                meetingId,
                attendeeRequest.GuestsNumber));

            return Ok();
        }

        [HttpDelete("{meetingId}/attendees/{attendeeId}")]
        [HasPermission(MeetingsPermissions.RemoveMeetingAttendee)]
        public async Task<IActionResult> RemoveMeetingAttendee(
            Guid meetingId,
            Guid attendeeId,
            RemoveMeetingAttendeeRequest request)
        {
            await _meetingsModule.ExecuteCommandAsync(
                new RemoveMeetingAttendeeCommand(meetingId, attendeeId, request.RemovingReason));

            return Ok();
        }

        [HttpPost("{meetingId}/notAttendees")]
        [HasPermission(MeetingsPermissions.AddNotAttendee)]
        public async Task<IActionResult> AddNotAttendee(Guid meetingId)
        {
            await _meetingsModule.ExecuteCommandAsync(new AddMeetingNotAttendeeCommand(meetingId));

            return Ok();
        }

        [HttpDelete("{meetingId}/notAttendees")]
        [HasPermission(MeetingsPermissions.ChangeNotAttendeeDecision)]
        public async Task<IActionResult> ChangeNotAttendeeDecision(Guid meetingId)
        {
            await _meetingsModule.ExecuteCommandAsync(new ChangeNotAttendeeDecisionCommand(meetingId));

            return Ok();
        }

        [HttpPost("{meetingId}/waitlistMembers")]
        [HasPermission(MeetingsPermissions.SignUpMemberToWaitlist)]
        public async Task<IActionResult> SignUpMemberToWaitlist(Guid meetingId)
        {
            await _meetingsModule.ExecuteCommandAsync(new SignUpMemberToWaitlistCommand(meetingId));

            return Ok();
        }

        [HttpDelete("{meetingId}/waitlistMembers")]
        [HasPermission(MeetingsPermissions.SignOffMemberFromWaitlist)]
        public async Task<IActionResult> SignOffMemberFromWaitlist(Guid meetingId)
        {
            await _meetingsModule.ExecuteCommandAsync(new SignOffMemberFromWaitlistCommand(meetingId));

            return Ok();
        }

        [HttpPost("{meetingId}/hosts")]
        [HasPermission(MeetingsPermissions.SetMeetingHostRole)]
        public async Task<IActionResult> SetMeetingHostRole(Guid meetingId, SetMeetingHostRequest request)
        {
            await _meetingsModule.ExecuteCommandAsync(new SetMeetingHostRoleCommand(request.AttendeeId, meetingId));

            return Ok();
        }

        [HttpPost("{meetingId}/attendees/attendeeRole")]
        [HasPermission(MeetingsPermissions.SetMeetingAttendeeRole)]
        public async Task<IActionResult> SetMeetingAttendeeRole(Guid meetingId, SetMeetingHostRequest request)
        {
            await _meetingsModule.ExecuteCommandAsync(new SetMeetingAttendeeRoleCommand(request.AttendeeId, meetingId));

            return Ok();
        }

        [HttpPatch("{meetingId}/cancel")]
        [HasPermission(MeetingsPermissions.CancelMeeting)]
        public async Task<IActionResult> CancelMeeting(Guid meetingId)
        {
            await _meetingsModule.ExecuteCommandAsync(new CancelMeetingCommand(meetingId));

            return Ok();
        }
    }
}