Rhenus.Core Requirements Document
=================================

# Overview

The Rhenus.Core project is the initial project of the Rhenus framework. The Rhenus framework is a set of libraries that try to provide a ditributed platform to applications with focus on an MMOG server backend. It is based on C# and CLI technology and aims to be platform independent, using both the .NET framework and Mono.

The Rhenus.Core project contains the Rhenus.Core namespace, the core component of the Rhenus project. It will build an executable that is containing a Kernel class controlling the startup and shutdown, as well as the event loop of a Rhenus node. 

- - -

# The Kernel

Every nodes kernel is providing means for services, managers or applications to schedule tasks for execution. This includes scheduling the task to run only once, recurringly, at a certain time or recurringly after a certain time. To run tasks concurrently the kernel makes use of the System.Threading.ThreadPool class.

# API

The kernel exposes a number of interfaces to the user. There are abstractions for 
  + Tasks,
  + an interface for access to the task scheduler,
  + handles for recurring tasks, 
  + abstractions for an identity that is running the task, 
  + and some more