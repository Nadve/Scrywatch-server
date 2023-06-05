# Scrywatch-server

>This is a back-end of Scrywatch. Web application for managing Magic: The Gathering card collection.
## Table of contents
* [General Info](#general-info)
* [Technologies Used](#technologies-used)
* [Features](#features)
* [Setup](#setup)
* [Usage](#usage)
* [Room For Improvement](#room-for-improvement)
* [Contact](#contact)

## General Info
Magic: The Gathering is a collectible card game spread all over the world. If
you want to play at a professional level, you need to constantly expand your
collection with new cards. The price of the cards is not regulated, so there can be
huge fluctuations from day to day. The goal of this thesis is to save collector’s
money when buying new cards and to raise the profit when selling by creating
a web application. Public API [scryfall.com](https://scryfall.com/docs/api) is used to get information about the
cards. The application will periodicaly check the prices of the cards and plot their
history in to a chart. User of the application can adjust the price change of which
he’ll be informed via email. 

## Technologies Used
* MSSQLServer
* ASP NET CORE
* Dapper
* Ardalis.SmartEnum
* MailKit

## Features
* Cards
  * Get names of all cards
  * Get all cards with given name
* Authentication
  * Register
  * Confirm email
  * Sign in
>user has to be signed in to have access to features below
* Interests
  * If the user is interested in buying/selling the card he can add the card to his collection
  * Cards can be removed from the collection
  * Every card in collection has a goal which user can modify
  * When the goal is met user will be notified via email

## Setup
* Publish the database defined in Scrywatch.Database or contact me and I'll provide database with year worth of data
* Modify the appSettings.json file located in Scrywatch.Api
  * Change value of ConnectionString.Default to your connection string
  * Change AuthToken.Key value
  * If you wish to send emails to users change the Mail section values to match your mail server. By default all emails will be sent to [Ethereal](https://ethereal.email/)

## Usage
`dotnet run`

## Contact
nadv.tom@gmail.com
