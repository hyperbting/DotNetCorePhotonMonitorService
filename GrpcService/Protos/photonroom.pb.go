// Code generated by protoc-gen-go. DO NOT EDIT.
// versions:
// 	protoc-gen-go v1.27.1
// 	protoc        v3.17.3
// source: photonroom.proto

package protos

import (
	protoreflect "google.golang.org/protobuf/reflect/protoreflect"
	protoimpl "google.golang.org/protobuf/runtime/protoimpl"
	timestamppb "google.golang.org/protobuf/types/known/timestamppb"
	reflect "reflect"
	sync "sync"
)

const (
	// Verify that this generated code is sufficiently up-to-date.
	_ = protoimpl.EnforceVersion(20 - protoimpl.MinVersion)
	// Verify that runtime/protoimpl is sufficiently up-to-date.
	_ = protoimpl.EnforceVersion(protoimpl.MaxVersion - 20)
)

type PhotonRegion int32

const (
	PhotonRegion_UNKNOWN PhotonRegion = 0
	PhotonRegion_JP      PhotonRegion = 1
	PhotonRegion_ASIA    PhotonRegion = 2
	PhotonRegion_AU      PhotonRegion = 3
	PhotonRegion_US      PhotonRegion = 4
	PhotonRegion_USW     PhotonRegion = 5
	PhotonRegion_CAE     PhotonRegion = 6
	PhotonRegion_EU      PhotonRegion = 30
	PhotonRegion_SA      PhotonRegion = 31
	PhotonRegion_RU      PhotonRegion = 32
	PhotonRegion_RUE     PhotonRegion = 33
	PhotonRegion_ZA      PhotonRegion = 34
	PhotonRegion_CN      PhotonRegion = 50
)

// Enum value maps for PhotonRegion.
var (
	PhotonRegion_name = map[int32]string{
		0:  "UNKNOWN",
		1:  "JP",
		2:  "ASIA",
		3:  "AU",
		4:  "US",
		5:  "USW",
		6:  "CAE",
		30: "EU",
		31: "SA",
		32: "RU",
		33: "RUE",
		34: "ZA",
		50: "CN",
	}
	PhotonRegion_value = map[string]int32{
		"UNKNOWN": 0,
		"JP":      1,
		"ASIA":    2,
		"AU":      3,
		"US":      4,
		"USW":     5,
		"CAE":     6,
		"EU":      30,
		"SA":      31,
		"RU":      32,
		"RUE":     33,
		"ZA":      34,
		"CN":      50,
	}
)

func (x PhotonRegion) Enum() *PhotonRegion {
	p := new(PhotonRegion)
	*p = x
	return p
}

func (x PhotonRegion) String() string {
	return protoimpl.X.EnumStringOf(x.Descriptor(), protoreflect.EnumNumber(x))
}

func (PhotonRegion) Descriptor() protoreflect.EnumDescriptor {
	return file_photonroom_proto_enumTypes[0].Descriptor()
}

func (PhotonRegion) Type() protoreflect.EnumType {
	return &file_photonroom_proto_enumTypes[0]
}

func (x PhotonRegion) Number() protoreflect.EnumNumber {
	return protoreflect.EnumNumber(x)
}

// Deprecated: Use PhotonRegion.Descriptor instead.
func (PhotonRegion) EnumDescriptor() ([]byte, []int) {
	return file_photonroom_proto_rawDescGZIP(), []int{0}
}

type RegionSetRequest struct {
	state         protoimpl.MessageState
	sizeCache     protoimpl.SizeCache
	unknownFields protoimpl.UnknownFields

	Region PhotonRegion `protobuf:"varint,1,opt,name=region,proto3,enum=photonroom.PhotonRegion" json:"region,omitempty"`
}

func (x *RegionSetRequest) Reset() {
	*x = RegionSetRequest{}
	if protoimpl.UnsafeEnabled {
		mi := &file_photonroom_proto_msgTypes[0]
		ms := protoimpl.X.MessageStateOf(protoimpl.Pointer(x))
		ms.StoreMessageInfo(mi)
	}
}

func (x *RegionSetRequest) String() string {
	return protoimpl.X.MessageStringOf(x)
}

func (*RegionSetRequest) ProtoMessage() {}

func (x *RegionSetRequest) ProtoReflect() protoreflect.Message {
	mi := &file_photonroom_proto_msgTypes[0]
	if protoimpl.UnsafeEnabled && x != nil {
		ms := protoimpl.X.MessageStateOf(protoimpl.Pointer(x))
		if ms.LoadMessageInfo() == nil {
			ms.StoreMessageInfo(mi)
		}
		return ms
	}
	return mi.MessageOf(x)
}

// Deprecated: Use RegionSetRequest.ProtoReflect.Descriptor instead.
func (*RegionSetRequest) Descriptor() ([]byte, []int) {
	return file_photonroom_proto_rawDescGZIP(), []int{0}
}

func (x *RegionSetRequest) GetRegion() PhotonRegion {
	if x != nil {
		return x.Region
	}
	return PhotonRegion_UNKNOWN
}

type RegionSetReply struct {
	state         protoimpl.MessageState
	sizeCache     protoimpl.SizeCache
	unknownFields protoimpl.UnknownFields

	Region      PhotonRegion           `protobuf:"varint,1,opt,name=region,proto3,enum=photonroom.PhotonRegion" json:"region,omitempty"`
	LastUpdated *timestamppb.Timestamp `protobuf:"bytes,10,opt,name=last_updated,json=lastUpdated,proto3" json:"last_updated,omitempty"`
}

func (x *RegionSetReply) Reset() {
	*x = RegionSetReply{}
	if protoimpl.UnsafeEnabled {
		mi := &file_photonroom_proto_msgTypes[1]
		ms := protoimpl.X.MessageStateOf(protoimpl.Pointer(x))
		ms.StoreMessageInfo(mi)
	}
}

func (x *RegionSetReply) String() string {
	return protoimpl.X.MessageStringOf(x)
}

func (*RegionSetReply) ProtoMessage() {}

func (x *RegionSetReply) ProtoReflect() protoreflect.Message {
	mi := &file_photonroom_proto_msgTypes[1]
	if protoimpl.UnsafeEnabled && x != nil {
		ms := protoimpl.X.MessageStateOf(protoimpl.Pointer(x))
		if ms.LoadMessageInfo() == nil {
			ms.StoreMessageInfo(mi)
		}
		return ms
	}
	return mi.MessageOf(x)
}

// Deprecated: Use RegionSetReply.ProtoReflect.Descriptor instead.
func (*RegionSetReply) Descriptor() ([]byte, []int) {
	return file_photonroom_proto_rawDescGZIP(), []int{1}
}

func (x *RegionSetReply) GetRegion() PhotonRegion {
	if x != nil {
		return x.Region
	}
	return PhotonRegion_UNKNOWN
}

func (x *RegionSetReply) GetLastUpdated() *timestamppb.Timestamp {
	if x != nil {
		return x.LastUpdated
	}
	return nil
}

type RoomListRequest struct {
	state         protoimpl.MessageState
	sizeCache     protoimpl.SizeCache
	unknownFields protoimpl.UnknownFields

	Region PhotonRegion `protobuf:"varint,1,opt,name=region,proto3,enum=photonroom.PhotonRegion" json:"region,omitempty"`
}

func (x *RoomListRequest) Reset() {
	*x = RoomListRequest{}
	if protoimpl.UnsafeEnabled {
		mi := &file_photonroom_proto_msgTypes[2]
		ms := protoimpl.X.MessageStateOf(protoimpl.Pointer(x))
		ms.StoreMessageInfo(mi)
	}
}

func (x *RoomListRequest) String() string {
	return protoimpl.X.MessageStringOf(x)
}

func (*RoomListRequest) ProtoMessage() {}

func (x *RoomListRequest) ProtoReflect() protoreflect.Message {
	mi := &file_photonroom_proto_msgTypes[2]
	if protoimpl.UnsafeEnabled && x != nil {
		ms := protoimpl.X.MessageStateOf(protoimpl.Pointer(x))
		if ms.LoadMessageInfo() == nil {
			ms.StoreMessageInfo(mi)
		}
		return ms
	}
	return mi.MessageOf(x)
}

// Deprecated: Use RoomListRequest.ProtoReflect.Descriptor instead.
func (*RoomListRequest) Descriptor() ([]byte, []int) {
	return file_photonroom_proto_rawDescGZIP(), []int{2}
}

func (x *RoomListRequest) GetRegion() PhotonRegion {
	if x != nil {
		return x.Region
	}
	return PhotonRegion_UNKNOWN
}

type RoomListReply struct {
	state         protoimpl.MessageState
	sizeCache     protoimpl.SizeCache
	unknownFields protoimpl.UnknownFields

	Region PhotonRegion `protobuf:"varint,1,opt,name=region,proto3,enum=photonroom.PhotonRegion" json:"region,omitempty"`
	// store roomName: headCount
	RoomUserCount []*InGameUserCount     `protobuf:"bytes,2,rep,name=room_user_count,json=roomUserCount,proto3" json:"room_user_count,omitempty"`
	LastUpdated   *timestamppb.Timestamp `protobuf:"bytes,10,opt,name=last_updated,json=lastUpdated,proto3" json:"last_updated,omitempty"`
}

func (x *RoomListReply) Reset() {
	*x = RoomListReply{}
	if protoimpl.UnsafeEnabled {
		mi := &file_photonroom_proto_msgTypes[3]
		ms := protoimpl.X.MessageStateOf(protoimpl.Pointer(x))
		ms.StoreMessageInfo(mi)
	}
}

func (x *RoomListReply) String() string {
	return protoimpl.X.MessageStringOf(x)
}

func (*RoomListReply) ProtoMessage() {}

func (x *RoomListReply) ProtoReflect() protoreflect.Message {
	mi := &file_photonroom_proto_msgTypes[3]
	if protoimpl.UnsafeEnabled && x != nil {
		ms := protoimpl.X.MessageStateOf(protoimpl.Pointer(x))
		if ms.LoadMessageInfo() == nil {
			ms.StoreMessageInfo(mi)
		}
		return ms
	}
	return mi.MessageOf(x)
}

// Deprecated: Use RoomListReply.ProtoReflect.Descriptor instead.
func (*RoomListReply) Descriptor() ([]byte, []int) {
	return file_photonroom_proto_rawDescGZIP(), []int{3}
}

func (x *RoomListReply) GetRegion() PhotonRegion {
	if x != nil {
		return x.Region
	}
	return PhotonRegion_UNKNOWN
}

func (x *RoomListReply) GetRoomUserCount() []*InGameUserCount {
	if x != nil {
		return x.RoomUserCount
	}
	return nil
}

func (x *RoomListReply) GetLastUpdated() *timestamppb.Timestamp {
	if x != nil {
		return x.LastUpdated
	}
	return nil
}

type InGameUserCount struct {
	state         protoimpl.MessageState
	sizeCache     protoimpl.SizeCache
	unknownFields protoimpl.UnknownFields

	RoomName string `protobuf:"bytes,1,opt,name=roomName,proto3" json:"roomName,omitempty"`
	Count    int32  `protobuf:"varint,2,opt,name=count,proto3" json:"count,omitempty"`
}

func (x *InGameUserCount) Reset() {
	*x = InGameUserCount{}
	if protoimpl.UnsafeEnabled {
		mi := &file_photonroom_proto_msgTypes[4]
		ms := protoimpl.X.MessageStateOf(protoimpl.Pointer(x))
		ms.StoreMessageInfo(mi)
	}
}

func (x *InGameUserCount) String() string {
	return protoimpl.X.MessageStringOf(x)
}

func (*InGameUserCount) ProtoMessage() {}

func (x *InGameUserCount) ProtoReflect() protoreflect.Message {
	mi := &file_photonroom_proto_msgTypes[4]
	if protoimpl.UnsafeEnabled && x != nil {
		ms := protoimpl.X.MessageStateOf(protoimpl.Pointer(x))
		if ms.LoadMessageInfo() == nil {
			ms.StoreMessageInfo(mi)
		}
		return ms
	}
	return mi.MessageOf(x)
}

// Deprecated: Use InGameUserCount.ProtoReflect.Descriptor instead.
func (*InGameUserCount) Descriptor() ([]byte, []int) {
	return file_photonroom_proto_rawDescGZIP(), []int{4}
}

func (x *InGameUserCount) GetRoomName() string {
	if x != nil {
		return x.RoomName
	}
	return ""
}

func (x *InGameUserCount) GetCount() int32 {
	if x != nil {
		return x.Count
	}
	return 0
}

var File_photonroom_proto protoreflect.FileDescriptor

var file_photonroom_proto_rawDesc = []byte{
	0x0a, 0x10, 0x70, 0x68, 0x6f, 0x74, 0x6f, 0x6e, 0x72, 0x6f, 0x6f, 0x6d, 0x2e, 0x70, 0x72, 0x6f,
	0x74, 0x6f, 0x12, 0x0a, 0x70, 0x68, 0x6f, 0x74, 0x6f, 0x6e, 0x72, 0x6f, 0x6f, 0x6d, 0x1a, 0x1f,
	0x67, 0x6f, 0x6f, 0x67, 0x6c, 0x65, 0x2f, 0x70, 0x72, 0x6f, 0x74, 0x6f, 0x62, 0x75, 0x66, 0x2f,
	0x54, 0x69, 0x6d, 0x65, 0x73, 0x74, 0x61, 0x6d, 0x70, 0x2e, 0x70, 0x72, 0x6f, 0x74, 0x6f, 0x22,
	0x44, 0x0a, 0x10, 0x52, 0x65, 0x67, 0x69, 0x6f, 0x6e, 0x53, 0x65, 0x74, 0x52, 0x65, 0x71, 0x75,
	0x65, 0x73, 0x74, 0x12, 0x30, 0x0a, 0x06, 0x72, 0x65, 0x67, 0x69, 0x6f, 0x6e, 0x18, 0x01, 0x20,
	0x01, 0x28, 0x0e, 0x32, 0x18, 0x2e, 0x70, 0x68, 0x6f, 0x74, 0x6f, 0x6e, 0x72, 0x6f, 0x6f, 0x6d,
	0x2e, 0x50, 0x68, 0x6f, 0x74, 0x6f, 0x6e, 0x52, 0x65, 0x67, 0x69, 0x6f, 0x6e, 0x52, 0x06, 0x72,
	0x65, 0x67, 0x69, 0x6f, 0x6e, 0x22, 0x81, 0x01, 0x0a, 0x0e, 0x52, 0x65, 0x67, 0x69, 0x6f, 0x6e,
	0x53, 0x65, 0x74, 0x52, 0x65, 0x70, 0x6c, 0x79, 0x12, 0x30, 0x0a, 0x06, 0x72, 0x65, 0x67, 0x69,
	0x6f, 0x6e, 0x18, 0x01, 0x20, 0x01, 0x28, 0x0e, 0x32, 0x18, 0x2e, 0x70, 0x68, 0x6f, 0x74, 0x6f,
	0x6e, 0x72, 0x6f, 0x6f, 0x6d, 0x2e, 0x50, 0x68, 0x6f, 0x74, 0x6f, 0x6e, 0x52, 0x65, 0x67, 0x69,
	0x6f, 0x6e, 0x52, 0x06, 0x72, 0x65, 0x67, 0x69, 0x6f, 0x6e, 0x12, 0x3d, 0x0a, 0x0c, 0x6c, 0x61,
	0x73, 0x74, 0x5f, 0x75, 0x70, 0x64, 0x61, 0x74, 0x65, 0x64, 0x18, 0x0a, 0x20, 0x01, 0x28, 0x0b,
	0x32, 0x1a, 0x2e, 0x67, 0x6f, 0x6f, 0x67, 0x6c, 0x65, 0x2e, 0x70, 0x72, 0x6f, 0x74, 0x6f, 0x62,
	0x75, 0x66, 0x2e, 0x54, 0x69, 0x6d, 0x65, 0x73, 0x74, 0x61, 0x6d, 0x70, 0x52, 0x0b, 0x6c, 0x61,
	0x73, 0x74, 0x55, 0x70, 0x64, 0x61, 0x74, 0x65, 0x64, 0x22, 0x43, 0x0a, 0x0f, 0x52, 0x6f, 0x6f,
	0x6d, 0x4c, 0x69, 0x73, 0x74, 0x52, 0x65, 0x71, 0x75, 0x65, 0x73, 0x74, 0x12, 0x30, 0x0a, 0x06,
	0x72, 0x65, 0x67, 0x69, 0x6f, 0x6e, 0x18, 0x01, 0x20, 0x01, 0x28, 0x0e, 0x32, 0x18, 0x2e, 0x70,
	0x68, 0x6f, 0x74, 0x6f, 0x6e, 0x72, 0x6f, 0x6f, 0x6d, 0x2e, 0x50, 0x68, 0x6f, 0x74, 0x6f, 0x6e,
	0x52, 0x65, 0x67, 0x69, 0x6f, 0x6e, 0x52, 0x06, 0x72, 0x65, 0x67, 0x69, 0x6f, 0x6e, 0x22, 0xc5,
	0x01, 0x0a, 0x0d, 0x52, 0x6f, 0x6f, 0x6d, 0x4c, 0x69, 0x73, 0x74, 0x52, 0x65, 0x70, 0x6c, 0x79,
	0x12, 0x30, 0x0a, 0x06, 0x72, 0x65, 0x67, 0x69, 0x6f, 0x6e, 0x18, 0x01, 0x20, 0x01, 0x28, 0x0e,
	0x32, 0x18, 0x2e, 0x70, 0x68, 0x6f, 0x74, 0x6f, 0x6e, 0x72, 0x6f, 0x6f, 0x6d, 0x2e, 0x50, 0x68,
	0x6f, 0x74, 0x6f, 0x6e, 0x52, 0x65, 0x67, 0x69, 0x6f, 0x6e, 0x52, 0x06, 0x72, 0x65, 0x67, 0x69,
	0x6f, 0x6e, 0x12, 0x43, 0x0a, 0x0f, 0x72, 0x6f, 0x6f, 0x6d, 0x5f, 0x75, 0x73, 0x65, 0x72, 0x5f,
	0x63, 0x6f, 0x75, 0x6e, 0x74, 0x18, 0x02, 0x20, 0x03, 0x28, 0x0b, 0x32, 0x1b, 0x2e, 0x70, 0x68,
	0x6f, 0x74, 0x6f, 0x6e, 0x72, 0x6f, 0x6f, 0x6d, 0x2e, 0x49, 0x6e, 0x47, 0x61, 0x6d, 0x65, 0x55,
	0x73, 0x65, 0x72, 0x43, 0x6f, 0x75, 0x6e, 0x74, 0x52, 0x0d, 0x72, 0x6f, 0x6f, 0x6d, 0x55, 0x73,
	0x65, 0x72, 0x43, 0x6f, 0x75, 0x6e, 0x74, 0x12, 0x3d, 0x0a, 0x0c, 0x6c, 0x61, 0x73, 0x74, 0x5f,
	0x75, 0x70, 0x64, 0x61, 0x74, 0x65, 0x64, 0x18, 0x0a, 0x20, 0x01, 0x28, 0x0b, 0x32, 0x1a, 0x2e,
	0x67, 0x6f, 0x6f, 0x67, 0x6c, 0x65, 0x2e, 0x70, 0x72, 0x6f, 0x74, 0x6f, 0x62, 0x75, 0x66, 0x2e,
	0x54, 0x69, 0x6d, 0x65, 0x73, 0x74, 0x61, 0x6d, 0x70, 0x52, 0x0b, 0x6c, 0x61, 0x73, 0x74, 0x55,
	0x70, 0x64, 0x61, 0x74, 0x65, 0x64, 0x22, 0x43, 0x0a, 0x0f, 0x49, 0x6e, 0x47, 0x61, 0x6d, 0x65,
	0x55, 0x73, 0x65, 0x72, 0x43, 0x6f, 0x75, 0x6e, 0x74, 0x12, 0x1a, 0x0a, 0x08, 0x72, 0x6f, 0x6f,
	0x6d, 0x4e, 0x61, 0x6d, 0x65, 0x18, 0x01, 0x20, 0x01, 0x28, 0x09, 0x52, 0x08, 0x72, 0x6f, 0x6f,
	0x6d, 0x4e, 0x61, 0x6d, 0x65, 0x12, 0x14, 0x0a, 0x05, 0x63, 0x6f, 0x75, 0x6e, 0x74, 0x18, 0x02,
	0x20, 0x01, 0x28, 0x05, 0x52, 0x05, 0x63, 0x6f, 0x75, 0x6e, 0x74, 0x2a, 0x80, 0x01, 0x0a, 0x0c,
	0x50, 0x68, 0x6f, 0x74, 0x6f, 0x6e, 0x52, 0x65, 0x67, 0x69, 0x6f, 0x6e, 0x12, 0x0b, 0x0a, 0x07,
	0x55, 0x4e, 0x4b, 0x4e, 0x4f, 0x57, 0x4e, 0x10, 0x00, 0x12, 0x06, 0x0a, 0x02, 0x4a, 0x50, 0x10,
	0x01, 0x12, 0x08, 0x0a, 0x04, 0x41, 0x53, 0x49, 0x41, 0x10, 0x02, 0x12, 0x06, 0x0a, 0x02, 0x41,
	0x55, 0x10, 0x03, 0x12, 0x06, 0x0a, 0x02, 0x55, 0x53, 0x10, 0x04, 0x12, 0x07, 0x0a, 0x03, 0x55,
	0x53, 0x57, 0x10, 0x05, 0x12, 0x07, 0x0a, 0x03, 0x43, 0x41, 0x45, 0x10, 0x06, 0x12, 0x06, 0x0a,
	0x02, 0x45, 0x55, 0x10, 0x1e, 0x12, 0x06, 0x0a, 0x02, 0x53, 0x41, 0x10, 0x1f, 0x12, 0x06, 0x0a,
	0x02, 0x52, 0x55, 0x10, 0x20, 0x12, 0x07, 0x0a, 0x03, 0x52, 0x55, 0x45, 0x10, 0x21, 0x12, 0x06,
	0x0a, 0x02, 0x5a, 0x41, 0x10, 0x22, 0x12, 0x06, 0x0a, 0x02, 0x43, 0x4e, 0x10, 0x32, 0x32, 0xab,
	0x01, 0x0a, 0x10, 0x52, 0x65, 0x70, 0x6f, 0x72, 0x74, 0x50, 0x68, 0x6f, 0x74, 0x6f, 0x6e, 0x52,
	0x6f, 0x6f, 0x6d, 0x12, 0x4c, 0x0a, 0x10, 0x52, 0x65, 0x71, 0x75, 0x65, 0x73, 0x74, 0x52, 0x65,
	0x67, 0x69, 0x6f, 0x6e, 0x53, 0x65, 0x74, 0x12, 0x1c, 0x2e, 0x70, 0x68, 0x6f, 0x74, 0x6f, 0x6e,
	0x72, 0x6f, 0x6f, 0x6d, 0x2e, 0x52, 0x65, 0x67, 0x69, 0x6f, 0x6e, 0x53, 0x65, 0x74, 0x52, 0x65,
	0x71, 0x75, 0x65, 0x73, 0x74, 0x1a, 0x1a, 0x2e, 0x70, 0x68, 0x6f, 0x74, 0x6f, 0x6e, 0x72, 0x6f,
	0x6f, 0x6d, 0x2e, 0x52, 0x65, 0x67, 0x69, 0x6f, 0x6e, 0x53, 0x65, 0x74, 0x52, 0x65, 0x70, 0x6c,
	0x79, 0x12, 0x49, 0x0a, 0x0f, 0x52, 0x65, 0x71, 0x75, 0x65, 0x73, 0x74, 0x52, 0x6f, 0x6f, 0x6d,
	0x4c, 0x69, 0x73, 0x74, 0x12, 0x1b, 0x2e, 0x70, 0x68, 0x6f, 0x74, 0x6f, 0x6e, 0x72, 0x6f, 0x6f,
	0x6d, 0x2e, 0x52, 0x6f, 0x6f, 0x6d, 0x4c, 0x69, 0x73, 0x74, 0x52, 0x65, 0x71, 0x75, 0x65, 0x73,
	0x74, 0x1a, 0x19, 0x2e, 0x70, 0x68, 0x6f, 0x74, 0x6f, 0x6e, 0x72, 0x6f, 0x6f, 0x6d, 0x2e, 0x52,
	0x6f, 0x6f, 0x6d, 0x4c, 0x69, 0x73, 0x74, 0x52, 0x65, 0x70, 0x6c, 0x79, 0x42, 0x65, 0x5a, 0x47,
	0x67, 0x69, 0x74, 0x68, 0x75, 0x62, 0x2e, 0x63, 0x6f, 0x6d, 0x2f, 0x68, 0x79, 0x70, 0x65, 0x72,
	0x62, 0x74, 0x69, 0x6e, 0x67, 0x2f, 0x64, 0x6f, 0x74, 0x6e, 0x65, 0x74, 0x63, 0x6f, 0x72, 0x65,
	0x70, 0x68, 0x6f, 0x74, 0x6f, 0x6e, 0x6d, 0x6f, 0x6e, 0x69, 0x74, 0x6f, 0x72, 0x73, 0x65, 0x72,
	0x76, 0x69, 0x63, 0x65, 0x2f, 0x67, 0x72, 0x70, 0x63, 0x73, 0x65, 0x72, 0x76, 0x69, 0x63, 0x65,
	0x2f, 0x70, 0x72, 0x6f, 0x74, 0x6f, 0x73, 0xaa, 0x02, 0x19, 0x50, 0x68, 0x6f, 0x74, 0x6f, 0x6e,
	0x52, 0x6f, 0x6f, 0x6d, 0x4c, 0x69, 0x73, 0x74, 0x47, 0x72, 0x70, 0x63, 0x53, 0x65, 0x72, 0x76,
	0x69, 0x63, 0x65, 0x62, 0x06, 0x70, 0x72, 0x6f, 0x74, 0x6f, 0x33,
}

var (
	file_photonroom_proto_rawDescOnce sync.Once
	file_photonroom_proto_rawDescData = file_photonroom_proto_rawDesc
)

func file_photonroom_proto_rawDescGZIP() []byte {
	file_photonroom_proto_rawDescOnce.Do(func() {
		file_photonroom_proto_rawDescData = protoimpl.X.CompressGZIP(file_photonroom_proto_rawDescData)
	})
	return file_photonroom_proto_rawDescData
}

var file_photonroom_proto_enumTypes = make([]protoimpl.EnumInfo, 1)
var file_photonroom_proto_msgTypes = make([]protoimpl.MessageInfo, 5)
var file_photonroom_proto_goTypes = []interface{}{
	(PhotonRegion)(0),             // 0: photonroom.PhotonRegion
	(*RegionSetRequest)(nil),      // 1: photonroom.RegionSetRequest
	(*RegionSetReply)(nil),        // 2: photonroom.RegionSetReply
	(*RoomListRequest)(nil),       // 3: photonroom.RoomListRequest
	(*RoomListReply)(nil),         // 4: photonroom.RoomListReply
	(*InGameUserCount)(nil),       // 5: photonroom.InGameUserCount
	(*timestamppb.Timestamp)(nil), // 6: google.protobuf.Timestamp
}
var file_photonroom_proto_depIdxs = []int32{
	0, // 0: photonroom.RegionSetRequest.region:type_name -> photonroom.PhotonRegion
	0, // 1: photonroom.RegionSetReply.region:type_name -> photonroom.PhotonRegion
	6, // 2: photonroom.RegionSetReply.last_updated:type_name -> google.protobuf.Timestamp
	0, // 3: photonroom.RoomListRequest.region:type_name -> photonroom.PhotonRegion
	0, // 4: photonroom.RoomListReply.region:type_name -> photonroom.PhotonRegion
	5, // 5: photonroom.RoomListReply.room_user_count:type_name -> photonroom.InGameUserCount
	6, // 6: photonroom.RoomListReply.last_updated:type_name -> google.protobuf.Timestamp
	1, // 7: photonroom.ReportPhotonRoom.RequestRegionSet:input_type -> photonroom.RegionSetRequest
	3, // 8: photonroom.ReportPhotonRoom.RequestRoomList:input_type -> photonroom.RoomListRequest
	2, // 9: photonroom.ReportPhotonRoom.RequestRegionSet:output_type -> photonroom.RegionSetReply
	4, // 10: photonroom.ReportPhotonRoom.RequestRoomList:output_type -> photonroom.RoomListReply
	9, // [9:11] is the sub-list for method output_type
	7, // [7:9] is the sub-list for method input_type
	7, // [7:7] is the sub-list for extension type_name
	7, // [7:7] is the sub-list for extension extendee
	0, // [0:7] is the sub-list for field type_name
}

func init() { file_photonroom_proto_init() }
func file_photonroom_proto_init() {
	if File_photonroom_proto != nil {
		return
	}
	if !protoimpl.UnsafeEnabled {
		file_photonroom_proto_msgTypes[0].Exporter = func(v interface{}, i int) interface{} {
			switch v := v.(*RegionSetRequest); i {
			case 0:
				return &v.state
			case 1:
				return &v.sizeCache
			case 2:
				return &v.unknownFields
			default:
				return nil
			}
		}
		file_photonroom_proto_msgTypes[1].Exporter = func(v interface{}, i int) interface{} {
			switch v := v.(*RegionSetReply); i {
			case 0:
				return &v.state
			case 1:
				return &v.sizeCache
			case 2:
				return &v.unknownFields
			default:
				return nil
			}
		}
		file_photonroom_proto_msgTypes[2].Exporter = func(v interface{}, i int) interface{} {
			switch v := v.(*RoomListRequest); i {
			case 0:
				return &v.state
			case 1:
				return &v.sizeCache
			case 2:
				return &v.unknownFields
			default:
				return nil
			}
		}
		file_photonroom_proto_msgTypes[3].Exporter = func(v interface{}, i int) interface{} {
			switch v := v.(*RoomListReply); i {
			case 0:
				return &v.state
			case 1:
				return &v.sizeCache
			case 2:
				return &v.unknownFields
			default:
				return nil
			}
		}
		file_photonroom_proto_msgTypes[4].Exporter = func(v interface{}, i int) interface{} {
			switch v := v.(*InGameUserCount); i {
			case 0:
				return &v.state
			case 1:
				return &v.sizeCache
			case 2:
				return &v.unknownFields
			default:
				return nil
			}
		}
	}
	type x struct{}
	out := protoimpl.TypeBuilder{
		File: protoimpl.DescBuilder{
			GoPackagePath: reflect.TypeOf(x{}).PkgPath(),
			RawDescriptor: file_photonroom_proto_rawDesc,
			NumEnums:      1,
			NumMessages:   5,
			NumExtensions: 0,
			NumServices:   1,
		},
		GoTypes:           file_photonroom_proto_goTypes,
		DependencyIndexes: file_photonroom_proto_depIdxs,
		EnumInfos:         file_photonroom_proto_enumTypes,
		MessageInfos:      file_photonroom_proto_msgTypes,
	}.Build()
	File_photonroom_proto = out.File
	file_photonroom_proto_rawDesc = nil
	file_photonroom_proto_goTypes = nil
	file_photonroom_proto_depIdxs = nil
}
