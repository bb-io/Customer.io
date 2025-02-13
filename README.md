# Blackbird.io Customer.io

Blackbird is the new automation backbone for the language technology industry. Blackbird provides enterprise-scale automation and orchestration with a simple no-code/low-code platform. Blackbird enables ambitious organizations to identify, vet and automate as many processes as possible. Not just localization workflows, but any business and IT process. This repository represents an application that is deployable on Blackbird and usable inside the workflow editor.

## Introduction

<!-- begin docs -->

Customer.io is a platform that enables businesses to send targeted and personalized messages to their customers via various communication channels such as email, SMS, and push notifications. It allows businesses to create automated campaigns based on user behavior, preferences, and interactions with the company's website or app.

## Before setting up

Before you can connect you need to make sure that:

- You have a Customer.io account and have either admin or workspace manager rights.
- You have an API key for your Customer.io account. You can find your API credentials in Customer.io under _Experimental features_ > _Account Settings_ > _App API Keys_. More information about API keys can be found [here](https://customer.io/docs/accounts-and-workspaces/managing-credentials/).

## Connecting

1. Navigate to apps and search for Customer.io. If you cannot find Customer.io then click _Add App_ in the top right corner, select Customer.io and add the app to your Blackbird environment.
2. Click _Add Connection_.
3. Name your connection for future reference e.g. 'My Customer.io connection'.
4. Fill in the API key to the Customer.io instance you want to connect to.
7. Click _Connect_.

![CustomerioBlackbirdConnection](image/README/CustomerioBlackbirdConnection.png)

## Actions

- **Get translation of a broadcast message** Get information about a translation of message in a broadcast
- **Get translation of a newsletter** Get information about a translation of an individual newsletter. Also includes file as HTML document in output
- **Get translation of a transactional message** Get information about a translation of an individual transactional message
- **List snippets** List all snippets in the workspace
- **Update a translation of a broadcast message** Update a translation of a specific broadcast action
- **Update snippet** Update the name or value of a snippet
- **Update translation of a newsletter** Update the translation of a newsletter variant
- **Update translation of a transactional message** Update the body and other data of a specific language variant for a transactional message
- **Get a translation of a campaign message** Getting a translation of a campaign message
- **Update a translation of a campaign message** Updating a translation of a campaign message

## Polling 

- **On broadcast created or updated** Triggered when a new broadcast was created or updated
- **On newsletter created or updated** Triggered when a newsletter is created or updated
- **On campaign message created or updated** Triggered when a campaign message is created or updated
- **On transactional message created or updated** Triggered when a transactional message is created or updated


## Feedback

Do you want to use this app or do you have feedback on our implementation? Reach out to us using the [established channels](https://www.blackbird.io/) or create an issue.

<!-- end docs -->
