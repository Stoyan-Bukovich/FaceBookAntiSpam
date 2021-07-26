# Facebook Anti-Spam

Let's give back Facebook's Messenger spammers a little bit of their own medicine.

This code creates and sends a lots of web requests to particular url, using up spammer's server resources if left running for long time.

The code pretends to be a valid web browser of different devices with each web request.

This is an console application. To use it you would need to compile it first.

It does required dotnet core installed on your machine prior compiling it.

If you would like to use it for different urls, please modify the code as fits your case.

The code can be used in anonymous way via publicly available anononymous and highly anonymous proxy servers. To run the app with proxy servers usage, use ./infinite true in your terminal / console. The app will download list of proxy servers, test them out one by one, and use only the servers which are available / online. If no proxy server is available, the app will switch automaticaly to direct connection.
