//MS-BUG workaround
#include <cstdio>
#include <boost/filesystem.hpp>
#include <iostream>
#include <fstream>
#include "MessageRouterConfiguration.hpp"
#include <boost/property_tree/xml_parser.hpp>
#include <boost/property_tree/ptree.hpp>
#include <boost/foreach.hpp>
#include <boost/foreach_fwd.hpp>

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

	// populate tree structure pt
	using boost::property_tree::ptree;
	ptree configurationFileTree;
	boost::property_tree::read_xml(_expectedFileName, configurationFileTree);
	
	ptree configurationNode = configurationFileTree.get_child("configuration");
	
	ptree messagingNode = configurationNode.get_child("messaging");
	
	ptree producersNode = messagingNode.get_child("producers");
	ptree subscribersNode = messagingNode.get_child("subscribers");

	BOOST_FOREACH( ptree::value_type const& producer, producersNode ) 
	{
		if( producer.first == "host" ) 
		{
			_allowedProducers.push_back (producer.second.get<std::string>("<xmlattr>.name"));
	    }
	}

	BOOST_FOREACH( ptree::value_type const& subscriber, subscribersNode ) 
	{
		if( subscriber.first == "host" ) 
		{
			_allowedSubscribers.push_back (subscriber.second.get<std::string>("<xmlattr>.name"));
	    }
	}

	ptree serverConfigurationNode = configurationNode.get_child("server");

	_serverAddress = serverConfigurationNode.get<std::string>("<xmlattr>.address");
	_serverPort = serverConfigurationNode.get<int>("<xmlattr>.port");
	_serverThreads = serverConfigurationNode.get<int>("<xmlattr>.threads");
}

void Configuration::MessageRouterConfiguration::ConfigurationToConsole ()
{
	std::cout << "\nAllowed subscriber hosts:\n";
	for (int i = 0; i < _allowedSubscribers.size(); i++)
	{
		std::cout << " - ";
		std::cout << _allowedSubscribers[i];
		std::cout << "\n";
	}
	
	std::cout << "\nAllowed producer hosts:\n";
	for (int i = 0; i < _allowedProducers.size(); i++)
	{
		std::cout << " - ";
		std::cout << _allowedProducers[i];
		std::cout << "\n";
	}

	std::cout << "\nServer setup:";
	std::cout << "\nHost : ";
	std::cout << _serverAddress;
	std::cout << "\nPort : ";
	std::cout << _serverPort;
	std::cout << "\nThreads : ";
	std::cout << _serverThreads;
	std::cout << "\n";
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

