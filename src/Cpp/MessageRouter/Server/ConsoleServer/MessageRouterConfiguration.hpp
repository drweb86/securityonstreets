//
// mime_types.hpp
// ~~~~~~~~~~~~~~
//
// Copyright (c) 2003-2012 Christopher M. Kohlhoff (chris at kohlhoff dot com)
//
// Distributed under the Boost Software License, Version 1.0. (See accompanying
// file LICENSE_1_0.txt or copy at http://www.boost.org/LICENSE_1_0.txt)
//

//MS-BUG workaround
#ifndef MessageRouterConfiguration_hpp_
#define MessageRouterConfiguration_hpp_

#include <cstdio>
#include <boost/filesystem.hpp>

namespace Configuration 
{
	class MessageRouterConfiguration;
}

class Configuration::MessageRouterConfiguration 
{
	private:
		char* _expectedFileName;
		std::vector<std::string> _allowedProducers;
		std::vector<std::string> _allowedSubscribers;
		std::string _serverAddress;
		int _serverPort;
		int _serverThreads;

	public:
		MessageRouterConfiguration();
		void Load ();
		void ConfigurationToConsole ();
		bool MessageProducerHostAllowed(char* host);
		bool MessageConsumerHostAllowed(char* host);
};

#endif