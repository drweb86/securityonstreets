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
#include <cstdio>
#include <boost/filesystem.hpp>
#include "MessageRouterConfiguration.hpp"

Configuration::MessageRouterConfiguration::MessageRouterConfiguration()
{
	_expectedFileName = "MessageBus.xml";
}

void Configuration::MessageRouterConfiguration::Load()
{
	if (!boost::filesystem::exists(_expectedFileName))
	{
		throw new std::exception("Configuration file 'MessageBus.xml' is missing.");
	}

	//TODO: read file

	//TODO: add configuration nodes
}

bool Configuration::MessageRouterConfiguration::MessageProducerHostAllowed(char* host)
{
	//TODO:
	return false;
}

bool Configuration::MessageRouterConfiguration::MessageConsumerHostAllowed(char* host)
{
	//TODO:
	return false;
}

