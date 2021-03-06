FEEDBACK


# Setup

Configure the service with the appropriate permissions

  * bot: appid secret
  * vsts: url password

When you ping the service, it responds whether the credentials are configured.

    /ping


# Feedbacks

Read the (top 10) feedbacks from users

    /list

Ignore the feedback with no action

    /ignore #20

Promote the feedback to a new formal bug

    /promote #14 'arda.main: menu complexity turns to low'

Promote the feedback to a known bug
    
    /promote #15 --bug 101


# Events

Receive webhook calls (HTTP POST), transform it into a message, and post it to 
the current communication channel.

## Examples 

Configure an event

    /event add bug-fixed --channel 'bug fixed (initiated by {user})'

Configure a webhook endpoint for receiving events

    /event add arda-bug-report --channel 'message template from {user}'

Update the current message (no change in URL)

    /event update arda-bug-report --channel 'ok'

List the conversations

    /event list

Drop the conversation

    /event drop arda-bug-report
    
    
# Debug

/debug dump

/debug whoami


V2
=====

# Feedback Notifications

Send a message to a conversation

    /notify #45 'hello, thanks for your feedback'


# Triggers

Associate triggers with actions

  * Feedback_ignored
  * Feedback_promoted
  * Bug_created
  * Bug_updated
  * Bug_fixed
  * Vsts_webhooks

Reply back to the channel

    /event trigger vsts:bug_updated notify 'hello, bug {bug} updated'

Action: call web api using registered endpoint

    /event trigger vsts:bug_fixed http 'http://blablabla.com/?{user}'

General tasks completed

    /event trigger vsts:task_completed http 'http://blablabla.com/?{user}'

Clear action

    /event trigger vsts:bug_fixed --clear


# Security

Take ownership control

    /admin owner <password>

Give admin control

    /admin grant user

Revoke admin control

    /admin revoke user

