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

### Unified
- **Download Content**: Downloads content based on the specified ID. To use this action, you must specify the content type to which the content is assigned. Currently, all supported content types are: **Transactional Message, Broadcast Message, Campaign Message, and Newsletter**.
- **Upload Content**: Uploads content based on the specified ID. This action is used in **pair with `Download Content`** to upload a translated HTML file and update the translation for the specified language.

## Polling 

- **On broadcast created or updated** Triggered when a new broadcast was created or updated
- **On newsletter created or updated** Triggered when a newsletter is created or updated
- **On campaign message created or updated** Triggered when a campaign message is created or updated
- **On transactional message created or updated** Triggered when a transactional message is created or updated

## Known Customer.io API limitations

### Drag-and-Drop editor content cannot be translated

Customer.io's API does not support translating content created with their drag-and-drop editor. This is an API limitation that affects all newsletters and broadcast messages created using the drag-and-drop editor.

Unfortunately, the API does not provide a way to automatically notify you if content was created using the drag-and-drop editor.

**How to identify this issue:**

When attempting to download or update translations, the body content may not be retrieved or updated correctly.

**Workaround:**

To enable translation of newsletter content:

1. Export the HTML from the English version of your email
2. Create a new broadcast or newsletter in Customer.io
3. Select **"Code"** as the content type (not drag-and-drop)
4. Paste the exported HTML into the Code editor
5. Copy the title and pre-header from the original newsletter
6. Use Blackbird to download and translate the content as normal

**Important:** While your content team can continue authoring emails using the drag-and-drop editor, they will need to manually copy the content to a "Code" email before it can be localized through Blackbird or any other automation.

This limitation applies to all email messages created with drag-and-drop editor:
- Broadcast messages
- Newsletters
- Campaign messages

## Feedback

Do you want to use this app or do you have feedback on our implementation? Reach out to us using the [established channels](https://www.blackbird.io/) or create an issue.

<!-- end docs -->
